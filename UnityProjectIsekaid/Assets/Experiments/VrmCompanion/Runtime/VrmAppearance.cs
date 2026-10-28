using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniVRM10;
using UnityEngine;

namespace Isekaid.ExperimentalVrm
{
    public interface ICompanionExpressions
    {
        bool RequestExpression(string name, float weight);
    }

    public sealed class VrmAppearance : MonoBehaviour, ICompanionExpressions
    {
        public Transform Anchor;
        public RuntimeAnimatorController TrialAnimations;
        public float FacingYaw;
        public Vrm10Instance Avatar { get; private set; }
        public string Status { get; private set; } = "Selecciona un VRM local. El archivo no se publica.";
        public string LocalPath = "";
        public string[] Expressions { get; private set; } = Array.Empty<string>();
        public float ImportMilliseconds { get; private set; }
        public bool PreviewingMotion { get; set; }
        private bool loading;
        private readonly Dictionary<string, ExpressionKey> keys = new Dictionary<string, ExpressionKey>(StringComparer.OrdinalIgnoreCase);

        public async Task LoadLocal(string path)
        {
            if (loading) throw new InvalidOperationException("Ya hay una importación en curso.");
            loading = true;
            Vrm10Instance candidate = null;
            try
            {
                byte[] bytes = VrmFileGuard.ReadLocal(path);
                var timer = System.Diagnostics.Stopwatch.StartNew();
                candidate = await Vrm10.LoadBytesAsync(bytes, canLoadVrm0X: false,
                    controlRigGenerationOption: ControlRigGenerationOption.Generate, showMeshes: false);
                if (this == null) { if (candidate != null) Destroy(candidate.gameObject); return; }
                if (candidate == null) throw new InvalidOperationException("UniVRM no devolvió un avatar.");
                var animator = candidate.GetComponent<Animator>();
                if (animator == null || animator.avatar == null || !animator.avatar.isValid || !animator.avatar.isHuman)
                    throw new InvalidOperationException("El avatar no tiene un Humanoid válido.");
                foreach (var renderer in candidate.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    foreach (var vertex in renderer.sharedMesh.vertices)
                        if (!float.IsFinite(vertex.x) || !float.IsFinite(vertex.y) || !float.IsFinite(vertex.z))
                            throw new InvalidOperationException("La malla contiene coordenadas inválidas.");
                    if (renderer.bounds.size.magnitude > 20) throw new InvalidOperationException("Escala fuera del límite de la prueba.");
                }
                foreach (var renderer in candidate.GetComponentsInChildren<Renderer>())
                    if (renderer.sharedMaterials.Any(m => m == null || m.shader == null || !m.shader.isSupported || m.shader.name == "Hidden/InternalErrorShader"))
                        throw new InvalidOperationException("Material o shader incompatible.");
                candidate.transform.SetParent(Anchor != null ? Anchor : transform, false);
                candidate.transform.localRotation = Quaternion.Euler(0, FacingYaw, 0);
                animator.runtimeAnimatorController = TrialAnimations;
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                if (Avatar != null) Destroy(Avatar.gameObject);
                Avatar = candidate; candidate = null;
                keys.Clear();
                foreach (var key in Avatar.Runtime.Expression.ExpressionKeys) keys[key.Name] = key;
                Expressions = keys.Keys.ToArray();
                var resources = Avatar.GetComponent<UniGLTF.RuntimeGltfInstance>();
                resources.ShowMeshes();
                resources.EnableUpdateWhenOffscreen();
                if (Anchor != null) foreach (var renderer in Anchor.GetComponentsInChildren<Renderer>())
                    if (!renderer.transform.IsChildOf(Avatar.transform)) renderer.enabled = false;
                ImportMilliseconds = (float)timer.Elapsed.TotalMilliseconds;
                Status = "VRM cargado localmente. Humanoid y materiales disponibles.";
            }
            catch (Exception error) { if (this != null) Status = error.Message; throw; }
            finally { if (candidate != null) Destroy(candidate.gameObject); loading = false; }
        }

        public bool RequestExpression(string name, float weight)
        {
            if (Avatar == null) return false;
            foreach (var key in keys.Values) Avatar.Runtime.Expression.SetWeight(key, 0f);
            if (!keys.TryGetValue(name ?? "", out var selected)) return false;
            Avatar.Runtime.Expression.SetWeight(selected, float.IsNaN(weight) || float.IsInfinity(weight) ? 0f : Mathf.Clamp01(weight));
            return true;
        }

        public void SetMotion(string name)
        {
            if (Avatar != null) Avatar.GetComponent<Animator>().CrossFade(name, .15f);
        }

        private async void LoadFromButton()
        {
            try { await LoadLocal(LocalPath); }
            catch (Exception)
            {
                // LoadLocal preserves the previous avatar and exposes the error through Status.
            }
        }

        private void OnDestroy() { if (Avatar != null) Destroy(Avatar.gameObject); }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(12, 12, 680, 190), GUI.skin.box);
            GUILayout.Label("Prueba VRM local — escena experimental");
            LocalPath = GUILayout.TextField(LocalPath);
            GUI.enabled = !loading;
            if (GUILayout.Button("Cargar VRM")) LoadFromButton();
            GUI.enabled = true;
            GUILayout.Label(Status);
            GUILayout.BeginHorizontal();
            foreach (var motion in new[] { "Idle", "Walk", "Run", "Turn" })
                if (GUILayout.Button(motion)) { PreviewingMotion = true; SetMotion(motion); }
            if (GUILayout.Button("Seguir movimiento")) PreviewingMotion = false;
            GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
            foreach (var expression in new[] { "neutral", "happy", "angry", "sad", "relaxed", "surprised", "blink", "aa" })
                if (GUILayout.Button(expression)) RequestExpression(expression, 1);
            GUILayout.EndHorizontal(); GUILayout.EndArea();
        }
    }
}
