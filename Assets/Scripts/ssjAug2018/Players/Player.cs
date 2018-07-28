using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.ssjAug2018.Camera;
using pdxpartyparrot.ssjAug2018.Items;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(FollowTarget))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class Player : NetworkActor
    {
#region Animations
        [Header("Animations")]

        [SerializeField]
        private Animator _animator;
#endregion

        public FollowTarget FollowTarget { get; private set; }

        public PlayerController PlayerController => (PlayerController)Controller;

        public CapsuleCollider CapsuleCollider => (CapsuleCollider)Collider;

        private AudioSource _audioSource;

        [CanBeNull]
        private Camera.Viewer _viewer;

        [CanBeNull]
        public override Core.Camera.Viewer Viewer => _viewer;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkIdentity.localPlayerAuthority = true;
            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;

#if UNITY_EDITOR
            if(!(Controller is PlayerController)) {
                Debug.LogError("Player controller must be a PlayerController!");
            }
#endif

            FollowTarget = GetComponent<FollowTarget>();

            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);

            PlayerManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;

            if(PlayerManager.HasInstance) {
                PlayerManager.Instance.Unregister(this);
            }
        }
#endregion

        private bool Initialize()
        {
            if(isLocalPlayer) {
                _viewer = (Camera.Viewer)ViewerManager.Instance.AcquireViewer();
                if(null == _viewer) {
                    return false;
                }
                _viewer.SetFocus(transform);
            }
            _viewer?.Initialize(this);

            PlayerController.Initialize(this);

            // TODO: encapsulate this somewhere better
            PlayerController.Rigidbody.mass = PlayerManager.Instance.PlayerData.Mass;
            PlayerController.Rigidbody.drag = PlayerManager.Instance.PlayerData.Drag;
            PlayerController.Rigidbody.angularDrag = PlayerManager.Instance.PlayerData.AngularDrag;

            return true;
        }

#region Commands
        [Command]
        public void CmdThrow(Vector3 origin, Vector3 direction, float speed)
        {
            Mail mail = ItemManager.Instance.GetMail();
            mail?.Throw(this, origin, direction, speed);
        }
#endregion

#region Callbacks
        public override void OnSpawn()
        {
            Debug.Log($"Spawning player (isLocalPlayer={isLocalPlayer})");

            Initialize();
        }
#endregion
    }
}
