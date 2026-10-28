using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniJSON;

namespace Isekaid.ExperimentalVrm
{
    public static class VrmFileGuard
    {
        public const int MaximumBytes = 32 * 1024 * 1024;

        public static byte[] ReadLocal(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Path.IsPathRooted(path) || path.StartsWith("\\\\", StringComparison.Ordinal) ||
                !string.Equals(Path.GetExtension(path), ".vrm", StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("Selecciona un archivo .vrm local con ruta absoluta.");
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream.Length < 28 || stream.Length > MaximumBytes)
                throw new InvalidDataException("El archivo debe contener entre 28 bytes y 32 MiB.");
            using var reader = new BinaryReader(stream);
            var bytes = reader.ReadBytes((int)stream.Length);
            if (stream.Position != stream.Length) throw new IOException("El archivo cambió durante la lectura.");
            Validate(bytes);
            return bytes;
        }

        public static void Validate(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 28 || bytes.Length > MaximumBytes ||
                U32(bytes, 0) != 0x46546c67 || U32(bytes, 4) != 2 || U32(bytes, 8) != bytes.Length)
                throw new InvalidDataException("Contenedor GLB inválido o demasiado grande.");
            long jsonSize = U32(bytes, 12);
            long binHeader = 20 + jsonSize;
            if (jsonSize < 2 || jsonSize > 4 * 1024 * 1024 || jsonSize % 4 != 0 ||
                U32(bytes, 16) != 0x4e4f534a || binHeader + 8 > bytes.Length ||
                U32(bytes, (int)binHeader + 4) != 0x004e4942 ||
                binHeader + 8 + U32(bytes, (int)binHeader) != bytes.Length)
                throw new InvalidDataException("Se requiere un GLB con JSON y un único BIN incorporado.");
            var json = new UTF8Encoding(false, true).GetString(bytes, 20, (int)jsonSize);
            int depth = 0; bool quoted = false; bool escaped = false;
            foreach (char c in json)
            {
                if (quoted) { if (escaped) escaped = false; else if (c == '\\') escaped = true; else if (c == '"') quoted = false; }
                else if (c == '"') quoted = true;
                else if (c == '{' || c == '[') { if (++depth > 64) throw new InvalidDataException("JSON demasiado profundo."); }
                else if (c == '}' || c == ']') { if (--depth < 0) throw new InvalidDataException("JSON inválido."); }
            }
            if (depth != 0 || quoted) throw new InvalidDataException("JSON incompleto.");
            var root = JsonParser.Parse(json);
            foreach (var node in root.Traverse())
            {
                if (!node.IsMap()) continue;
                var keys = new HashSet<string>(StringComparer.Ordinal);
                foreach (var item in node.ObjectItems())
                {
                    string key = item.Key.GetString();
                    if (!keys.Add(key)) throw new InvalidDataException("Claves JSON duplicadas.");
                    if (key == "uri") throw new InvalidDataException("Solo se aceptan recursos incorporados, sin URI externas o data URI.");
                    if (key == "count" && (item.Value.GetInt64() < 0 || item.Value.GetInt64() > 2000000))
                        throw new InvalidDataException("Accessor fuera de presupuesto.");
                }
            }
            if (root["asset"]["version"].GetString() != "2.0" || !root["extensions"].ContainsKey("VRMC_vrm") ||
                root["extensions"]["VRMC_vrm"]["specVersion"].GetString() != "1.0")
                throw new InvalidDataException("Esta prueba acepta únicamente VRM 1.0 estable.");
            Limit(root, "nodes", 512); Limit(root, "meshes", 32); Limit(root, "materials", 64);
            Limit(root, "images", 64); Limit(root, "accessors", 2048);
            if (root["buffers"].GetArrayCount() != 1 || root["buffers"][0]["byteLength"].GetInt64() < 0 ||
                root["buffers"][0]["byteLength"].GetInt64() > U32(bytes, (int)binHeader))
                throw new InvalidDataException("Se requiere un único buffer incorporado de longitud válida.");
            foreach (var view in root["bufferViews"].ArrayItems())
            {
                long offset = view.ContainsKey("byteOffset") ? view["byteOffset"].GetInt64() : 0;
                long length = view["byteLength"].GetInt64();
                long binLength = U32(bytes, (int)binHeader);
                if (view["buffer"].GetInt32() != 0 || offset < 0 || offset > binLength || length < 0 || length > binLength - offset)
                    throw new InvalidDataException("BufferView fuera del BIN incorporado.");
            }
            long accessorElements = 0;
            foreach (var accessor in root["accessors"].ArrayItems()) accessorElements += accessor["count"].GetInt64();
            if (accessorElements > 12000000) throw new InvalidDataException("Accessors por encima del presupuesto total.");
            var nodeList = new List<JsonNode>(root["nodes"].ArrayItems());
            var parents = new int[nodeList.Count]; var colors = new byte[nodeList.Count];
            Action<int, int> visit = null;
            visit = (index, level) =>
            {
                if (index < 0 || index >= nodeList.Count || colors[index] == 1 || level > 128)
                    throw new InvalidDataException("Jerarquía de nodos inválida o cíclica.");
                if (colors[index] == 2) return;
                colors[index] = 1;
                if (nodeList[index].ContainsKey("children")) foreach (var child in nodeList[index]["children"].ArrayItems())
                {
                    int next = child.GetInt32();
                    if (next < 0 || next >= nodeList.Count || ++parents[next] > 1) throw new InvalidDataException("Nodo con padres duplicados.");
                    visit(next, level + 1);
                }
                colors[index] = 2;
            };
            for (int i = 0; i < nodeList.Count; i++) visit(i, 0);
            if (root["extensions"].ContainsKey("VRMC_springBone"))
            {
                var springs = root["extensions"]["VRMC_springBone"];
                Limit(springs, "springs", 128); Limit(springs, "colliders", 256);
                long jointReferences = 0;
                if (springs.ContainsKey("springs")) foreach (var spring in springs["springs"].ArrayItems()) jointReferences += spring["joints"].GetArrayCount();
                if (jointReferences > 2048) throw new InvalidDataException("Spring Bones fuera de presupuesto.");
            }
            long vertices = 0, morphs = 0;
            foreach (var mesh in root["meshes"].ArrayItems()) foreach (var primitive in mesh["primitives"].ArrayItems())
            {
                int index = primitive["attributes"]["POSITION"].GetInt32();
                int count = root["accessors"][index]["count"].GetInt32();
                vertices += count;
                if (primitive.ContainsKey("targets")) morphs += (long)count * primitive["targets"].GetArrayCount();
            }
            if (vertices > 250000 || morphs > 12000000) throw new InvalidDataException("Geometría o morphs fuera de presupuesto.");
            long pixels = 0;
            foreach (var image in root["images"].ArrayItems())
            {
                if (image["mimeType"].GetString() != "image/png") throw new InvalidDataException("Esta prueba admite texturas PNG incorporadas.");
                var view = root["bufferViews"][image["bufferView"].GetInt32()];
                long offset = view.ContainsKey("byteOffset") ? view["byteOffset"].GetInt64() : 0;
                long length = view["byteLength"].GetInt64();
                long start = binHeader + 8 + offset;
                if (view["buffer"].GetInt32() != 0 || offset < 0 || length < 24 || start + length > bytes.Length ||
                    U32(bytes, (int)start) != 0x474e5089 || U32(bytes, (int)start + 12) != 0x52444849)
                    throw new InvalidDataException("Textura PNG o bufferView inválidos.");
                long width = Big32(bytes, (int)start + 16), height = Big32(bytes, (int)start + 20);
                if (width < 1 || height < 1 || width > 4096 || height > 4096) throw new InvalidDataException("Textura mayor de 4096 o vacía.");
                pixels += width * height;
            }
            if (pixels > 64L * 1024 * 1024) throw new InvalidDataException("Texturas por encima de 64 megapíxeles.");
        }

        private static uint U32(byte[] bytes, int offset) => BitConverter.ToUInt32(bytes, offset);
        private static uint Big32(byte[] b, int i) => ((uint)b[i] << 24) | ((uint)b[i + 1] << 16) | ((uint)b[i + 2] << 8) | b[i + 3];
        private static void Limit(JsonNode root, string name, int maximum)
        {
            if (root.ContainsKey(name) && root[name].GetArrayCount() > maximum) throw new InvalidDataException(name + " fuera de presupuesto.");
        }
    }
}
