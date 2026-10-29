using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEngine.AI;

namespace Isekaid
{
    public sealed class CompanionAppearance : MonoBehaviour
    {
        [SerializeField] private Renderer[] hair;
        [SerializeField] private Renderer[] eyes;
        [SerializeField] private Renderer[] top;
        [SerializeField] private Renderer[] trousers;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private GameObject catalogPrefab;
        [SerializeField] private RuntimeAnimatorController vrmAnimations;

        public VrmAppearance Importer { get; private set; }
        public AvatarSelection Selection { get; private set; } = new AvatarSelection();
        public string Status { get; private set; } = "";
        public bool Ready { get; private set; }
        public bool Busy { get; private set; }
        public bool RecoveredFallback { get; private set; }
        public GameObject Catalog { get; private set; }
        private Animator catalogAnimator;
        private NavMeshAgent agent;
        private CompanionController companion;
        private string motion;
        private CancellationTokenSource lifetime;

        private void Initialize()
        {
            if (Importer != null) return;
            if (visualRoot == null || catalogPrefab == null || vrmAnimations == null)
                throw new InvalidOperationException("La apariencia no tiene las referencias de catálogo y animación.");
            Catalog = Instantiate(catalogPrefab, visualRoot, false);
            foreach (var child in Catalog.GetComponentsInChildren<Transform>(true)) child.gameObject.layer = visualRoot.gameObject.layer;
            catalogAnimator = Catalog.GetComponentInChildren<Animator>();
            bool preview = GetComponent<CompanionController>() == null;
            if (catalogAnimator != null) catalogAnimator.updateMode = preview ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
            foreach (var renderer in visualRoot.GetComponentsInChildren<Renderer>())
                if (!renderer.transform.IsChildOf(Catalog.transform)) renderer.enabled = false;
            Importer = gameObject.AddComponent<VrmAppearance>();
            Importer.Anchor = visualRoot;
            Importer.TrialAnimations = vrmAnimations;
            Importer.ShowDiagnosticGUI = false;
            Importer.FitHeight = 1.7f;
            Importer.UseUnscaledTime = preview;
            lifetime = new CancellationTokenSource();
            companion = GetComponent<CompanionController>();
            agent = GetComponent<NavMeshAgent>();
            Ready = true;
        }

        private async void Start()
        {
            if (GetComponent<CompanionController>() == null) return;
            try { await LoadSaved(); }
            catch (OperationCanceledException) { }
            catch (Exception) { Status = "No se pudo preparar la apariencia. Abre el creador para recuperarla."; }
        }

        public async Task LoadSaved(CancellationToken cancellation = default, bool preserveOnFailure = false)
        {
            Initialize();
            var record = CompanionProfileStore.LoadRecord();
            Status = CompanionProfileStore.LastWarning;
            if (record.appearance.kind == "catalog") { SelectCatalog(); return; }
            Ready = false;
            try { await LoadVrm(AvatarFiles.ManagedPath(record.appearance), true, cancellation, record.appearance.hash); }
            catch (OperationCanceledException) { RecoveredFallback = !Importer.HasAvatar; throw; }
            catch (Exception)
            {
                if (preserveOnFailure) throw;
                SelectCatalog();
                RecoveredFallback = true;
                Status = "La copia privada no está disponible o está dañada. Se muestra Adventurer; el perfil se conserva. Reimporta o confirma el catálogo desde el creador.";
            }
            finally { Ready = true; }
        }

        public void SelectCatalog()
        {
            Initialize();
            if (Busy) throw new InvalidOperationException("Espera a que termine la importación cancelada.");
            Importer.Unload();
            Selection = new AvatarSelection();
            RecoveredFallback = false;
            Ready = true;
            motion = null;
            Status = "Adventurer · Dawn to Dusk Games · CC0-1.0. Calidad visual provisional de Fase 2.";
        }

        public async Task LoadVrm(string path, bool acknowledged, CancellationToken cancellation = default, string expectedHash = null)
        {
            Initialize();
            if (!acknowledged) throw new InvalidOperationException("Revisa los permisos y confirma el uso local antes de importar.");
            if (Busy) throw new InvalidOperationException("Ya hay una importación en curso.");
            Busy = true;
            try
            {
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, lifetime.Token);
                var bytes = VrmFileGuard.ReadLocal(path);
                var selection = AvatarSelection.FromBytes(bytes, acknowledged);
                if (expectedHash != null && selection.hash != expectedHash) throw new System.IO.InvalidDataException("La copia privada está dañada.");
                await Importer.LoadLocal(path, linked.Token, selection.hash);
                Selection = selection;
                RecoveredFallback = false;
                Ready = true;
                motion = null;
                Status = "VRM 1.0 preparado para uso local. Permisos declarados pendientes de revisión humana.";
            }
            finally { Busy = false; }
        }

        public void PreviewMotion(string value)
        {
            Initialize();
            if (Importer.HasAvatar) Importer.SetMotion(value);
            else if (catalogAnimator != null && catalogAnimator.HasState(0, Animator.StringToHash(value))) catalogAnimator.CrossFade(value, .15f);
        }

        public bool PreviewExpression(string value)
        {
            Initialize();
            if (Importer.HasAvatar) return Importer.RequestExpression(value, 1f);
            string[] channels;
            switch (value)
            {
                case "neutral": channels = Array.Empty<string>(); break;
                case "happy": channels = new[] { "Mouth_Smile_01", "Eyes_Smile_01" }; break;
                case "sad": channels = new[] { "Mouth_Sad_01" }; break;
                case "surprised": channels = new[] { "Eyes_Surprised_01", "Mouth_Oh" }; break;
                case "blink": channels = new[] { "RightEye_Close", "LeftEye_Close" }; break;
                case "aa": channels = new[] { "Mouth_Open" }; break;
                default: channels = null; break;
            }
            bool available = channels != null;
            foreach (var renderer in Catalog.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var mesh = renderer.sharedMesh;
                for (int i = 0; i < mesh.blendShapeCount; i++) renderer.SetBlendShapeWeight(i, 0);
                if (channels == null || mesh.blendShapeCount == 0) continue;
                foreach (string channel in channels)
                {
                    int index = mesh.GetBlendShapeIndex(channel);
                    if (index >= 0) renderer.SetBlendShapeWeight(index, 70); else available = false;
                }
            }
            return available;
        }

        private void Update()
        {
            if (!Ready || companion == null || agent == null) return;
            string next = companion.State.LifeState == CompanionLifeState.Active && companion.IsFollowing ?
                (agent.velocity.magnitude > 3f ? "Run" : agent.velocity.magnitude > .1f ? "Walk" : "Idle") : "Idle";
            if (motion == next) return;
            motion = next;
            PreviewMotion(next);
        }

        private void OnDestroy()
        {
            lifetime?.Cancel();
            lifetime?.Dispose();
        }

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private MaterialPropertyBlock properties;

        public void Apply(CompanionProfile profile)
        {
            if (profile == null || !profile.IsValid)
                return;
            for (int part = 0; part < CompanionPalette.Colors.Length; part++)
                SetColor(part, CompanionPalette.Colors[part][profile.GetColorIndex(part)]);
        }

        public void SetColor(int part, Color color)
        {
            Renderer[] targets;
            switch (part)
            {
                case 0: targets = hair; break;
                case 1: targets = eyes; break;
                case 2: targets = top; break;
                case 3: targets = trousers; break;
                default: return;
            }

            if (properties == null)
                properties = new MaterialPropertyBlock();

            foreach (var target in targets)
            {
                if (target == null)
                    continue;
                target.GetPropertyBlock(properties);
                properties.SetColor(BaseColor, color);
                target.SetPropertyBlock(properties);
            }
        }
    }
}
