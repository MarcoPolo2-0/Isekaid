using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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
        public bool ShowDiagnosticGUI = true;
        public float FitHeight;
        public bool UseUnscaledTime;
        public byte[] LoadedBytes { get; private set; }
        public Vrm10Instance Avatar { get; private set; }
        public bool HasAvatar => Avatar != null;
        public string Status { get; private set; } = "Selecciona un VRM local. El archivo no se publica.";
        public string LocalPath = "";
        public string[] Expressions { get; private set; } = Array.Empty<string>();
        public float ImportMilliseconds { get; private set; }
        public bool PreviewingMotion { get; set; }
        private bool loading;
        private bool destroyed;
        private GameObject avatarRoot;
        private Animator avatarAnimator;
        private Dictionary<string, ExpressionKey> keys = new Dictionary<string, ExpressionKey>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<Renderer, bool> originalVisibility = new Dictionary<Renderer, bool>();
#if UNITY_EDITOR
        public Action<string, Vrm10Instance> ImportStageProbe;
#endif

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void Checkpoint(string stage, Vrm10Instance candidate = null)
        {
#if UNITY_EDITOR
            ImportStageProbe?.Invoke(stage, candidate);
#endif
        }

        public async Task LoadLocal(string path, CancellationToken cancellation = default, string expectedHash = null)
        {
            if (loading) throw new InvalidOperationException("Ya hay una importación en curso.");
            loading = true;
            Vrm10Instance candidate = null;
            GameObject retired = null;
            GameObject candidateRoot = null;
            Vrm10FastSpringboneRuntimeStandalone springRuntime = null;
            var visibility = new Dictionary<Renderer, bool>();
            try
            {
                byte[] bytes = VrmFileGuard.ReadLocal(path);
                if (expectedHash != null && AvatarFiles.Hash(bytes) != expectedHash)
                    throw new System.IO.InvalidDataException("La copia privada del avatar está dañada.");
                cancellation.ThrowIfCancellationRequested();
                Checkpoint("ValidatedBytes");
                var timer = System.Diagnostics.Stopwatch.StartNew();
                // Retired avatars must not requeue disposed buffers through the shared spring combiner.
                springRuntime = new Vrm10FastSpringboneRuntimeStandalone();
                candidate = await Vrm10.LoadBytesAsync(bytes, canLoadVrm0X: false,
                    controlRigGenerationOption: ControlRigGenerationOption.Generate, showMeshes: false,
                    ct: cancellation, springboneRuntime: springRuntime);
                if (destroyed || this == null) throw new OperationCanceledException("El presentador fue destruido durante la importación.");
                cancellation.ThrowIfCancellationRequested();
                if (candidate == null) throw new InvalidOperationException("UniVRM no devolvió un avatar.");
                Checkpoint("Imported", candidate);
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
                // Build the normalized rig before moving the imported root into a rotated scene anchor.
                var runtime = candidate.Runtime;
                candidateRoot = candidate.gameObject;
                if (FitHeight > 0)
                {
                    candidateRoot = new GameObject("AvatarPresentation");
                    candidate.transform.SetParent(candidateRoot.transform, false);
                }
                candidateRoot.transform.SetParent(Anchor != null ? Anchor : transform, false);
                foreach (var child in candidate.GetComponentsInChildren<Transform>(true)) child.gameObject.layer = (Anchor != null ? Anchor : transform).gameObject.layer;
                candidateRoot.transform.localRotation = Quaternion.Euler(0, FacingYaw, 0);
                animator.runtimeAnimatorController = TrialAnimations;
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                animator.updateMode = UseUnscaledTime ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
                var nextKeys = new Dictionary<string, ExpressionKey>(StringComparer.OrdinalIgnoreCase);
                foreach (var key in candidate.Runtime.Expression.ExpressionKeys) nextKeys.Add(key.Name, key);
                var nextExpressions = nextKeys.Keys.ToArray();
                var resources = candidate.GetComponent<UniGLTF.RuntimeGltfInstance>();
                if (resources == null || resources.VisibleRenderers.Count == 0)
                    throw new InvalidOperationException("El candidato no posee recursos o renderers visibles.");
                var nextOriginalVisibility = new Dictionary<Renderer, bool>(originalVisibility);
                if (Anchor != null) foreach (var renderer in Anchor.GetComponentsInChildren<Renderer>())
                {
                    if (renderer.transform.IsChildOf(candidate.transform)) continue;
                    visibility.Add(renderer, renderer.enabled);
                    if ((Avatar == null || !renderer.transform.IsChildOf(Avatar.transform)) && !nextOriginalVisibility.ContainsKey(renderer))
                        nextOriginalVisibility.Add(renderer, renderer.enabled);
                }
                Checkpoint("Prepared", candidate);
                cancellation.ThrowIfCancellationRequested();
                resources.EnableUpdateWhenOffscreen();
                candidate.gameObject.SetActive(true);
                animator.Rebind();
                animator.Update(0f);
                if (FitHeight > 0)
                {
                    candidate.Runtime.Process();
                    await PreparedFrame(cancellation);
                    var bounds = VisibleBounds(candidate);
                    if (bounds.size.y < .1f) throw new InvalidOperationException("Altura inválida.");
                    candidateRoot.transform.localScale *= FitHeight / bounds.size.y;
                    animator.Update(0f);
                    candidate.Runtime.Process();
                    await PreparedFrame(cancellation);
                    bounds = VisibleBounds(candidate);
                    candidateRoot.transform.position += Vector3.up * ((Anchor != null ? Anchor : transform).position.y - bounds.min.y);
                    foreach (var collider in candidate.GetComponentsInChildren<Collider>()) collider.enabled = false;
                }
                resources.ShowMeshes();
                if (resources.VisibleRenderers.Any(r => r == null || !r.enabled || !r.gameObject.activeInHierarchy))
                    throw new InvalidOperationException("No se pudo activar la apariencia candidata.");
                Checkpoint("Activated", candidate);
                foreach (var renderer in visibility.Keys) renderer.enabled = false;
                Checkpoint("PreviousHidden", candidate);
                cancellation.ThrowIfCancellationRequested();

                // Publish the prepared presentation only after every fallible activation step succeeds.
                retired = avatarRoot;
                Avatar = candidate;
                avatarAnimator = animator;
                avatarRoot = candidateRoot;
                candidateRoot = null;
                springRuntime = null;
                LoadedBytes = bytes;
                keys = nextKeys;
                Expressions = nextExpressions;
                originalVisibility = nextOriginalVisibility;
                candidate = null;
                ImportMilliseconds = (float)timer.Elapsed.TotalMilliseconds;
                Status = "VRM cargado localmente. Humanoid y materiales disponibles.";
            }
            catch (Exception error)
            {
                RestoreVisibility(visibility);
                if (this != null) Status = error.Message;
                throw;
            }
            finally
            {
                if (candidate != null)
                {
                    if (candidateRoot == null) candidateRoot = candidate.gameObject;
                    candidateRoot.SetActive(false);
                    Destroy(candidateRoot);
                    springRuntime = null;
                }
                springRuntime?.Dispose();
                loading = false;
            }
            if (retired != null) Destroy(retired);
        }

        private static void RestoreVisibility(Dictionary<Renderer, bool> values)
        {
            foreach (var pair in values) if (pair.Key != null) pair.Key.enabled = pair.Value;
        }

        private static Bounds VisibleBounds(Vrm10Instance avatar)
        {
            var renderers = avatar.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) throw new InvalidOperationException("El avatar no tiene geometría visible.");
            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            if (!float.IsFinite(bounds.center.x) || !float.IsFinite(bounds.center.y) || !float.IsFinite(bounds.center.z) ||
                !float.IsFinite(bounds.size.x) || !float.IsFinite(bounds.size.y) || !float.IsFinite(bounds.size.z))
                throw new InvalidOperationException("El rig produce coordenadas inválidas.");
            return bounds;
        }

        private async Task PreparedFrame(CancellationToken cancellation)
        {
            int frame = Time.frameCount;
            do { await Task.Yield(); } while (frame == Time.frameCount);
            cancellation.ThrowIfCancellationRequested();
            if (destroyed || this == null) throw new OperationCanceledException("La vista previa ya no está disponible.");
        }

        public void Unload()
        {
            if (loading) throw new InvalidOperationException("Espera a que termine la importación antes de descargar.");
            ReleaseAvatar();
            Status = "Avatar descargado. Puedes importar otro archivo local.";
        }

        private void ReleaseAvatar()
        {
            if (avatarRoot != null) { avatarRoot.SetActive(false); Destroy(avatarRoot); }
            avatarRoot = null;
            Avatar = null;
            avatarAnimator = null;
            LoadedBytes = null;
            keys.Clear();
            Expressions = Array.Empty<string>();
            RestoreVisibility(originalVisibility);
            originalVisibility.Clear();
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
            AvatarLocomotion.Preview(avatarAnimator, name);
        }

        public void SetSpeed(float speed, float deltaTime)
        {
            AvatarLocomotion.SetSpeed(avatarAnimator, speed, deltaTime);
        }

        private async void LoadFromButton()
        {
            try { await LoadLocal(LocalPath); }
            catch (Exception)
            {
                // LoadLocal preserves the previous avatar and exposes the error through Status.
            }
        }

        private void OnDestroy() { destroyed = true; ReleaseAvatar(); }

        private void OnGUI()
        {
            if (!ShowDiagnosticGUI) return;
            GUILayout.BeginArea(new Rect(12, 12, 680, 190), GUI.skin.box);
            GUILayout.Label("Prueba VRM local — escena experimental");
            LocalPath = GUILayout.TextField(LocalPath);
            GUI.enabled = !loading;
            if (GUILayout.Button("Cargar VRM")) LoadFromButton();
            if (Avatar != null && GUILayout.Button("Descargar avatar")) Unload();
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
