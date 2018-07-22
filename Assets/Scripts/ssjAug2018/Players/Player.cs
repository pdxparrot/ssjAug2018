using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;

using pdxpartyparrot.ssjAug2018.Camera;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(FollowTarget))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class Player : NetworkActor
    {
#region Animations
        [Header("Animations")]

        [SerializeField]
        private Animator _animator;
#endregion

        public FollowTarget FollowTarget { get; private set; }

        public Collider Collider { get; private set; }

        public PlayerController PlayerController => (PlayerController)Controller;

        private AudioSource _audioSource;

        private Camera.Viewer _viewer;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkIdentity.localPlayerAuthority = true;

#if UNITY_EDITOR
            if(!(Controller is PlayerController)) {
                Debug.LogError("Player controller must be a PlayerController!");
            }
#endif

            FollowTarget = GetComponent<FollowTarget>();
            Collider = GetComponent<Collider>();

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

        public bool Initialize()
        {
            _viewer = (Camera.Viewer)ViewerManager.Instance.AcquireViewer();
            if(null == _viewer) {
                return false;
            }
            _viewer.SetFocus(transform);

            PlayerController.Initialize(this);

            return true;
        }

#region Callbacks
        public override void OnSpawn()
        {
            _viewer.Initialize(this);
        }
#endregion
    }
}
