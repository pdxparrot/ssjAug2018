using System.Collections;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Camera;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(FollowTarget))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class Player : NetworkActor
    {
#region Inventory
        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentLetterCount;

        public int CurrentLetterCount => _currentLetterCount;

        public bool CanThrowMail => CurrentLetterCount > 0;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private long _reloadCompleteTime;

        public bool IsReloading => _reloadCompleteTime > 0 && TimeManager.Instance.CurrentUnixMs <= _reloadCompleteTime;
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
            InitializeLocalPlayer();
            PlayerController.Initialize(this);

            // TODO: encapsulate this somewhere better
            PlayerController.Rigidbody.mass = PlayerManager.Instance.PlayerData.Mass;
            PlayerController.Rigidbody.drag = PlayerManager.Instance.PlayerData.Drag;
            PlayerController.Rigidbody.angularDrag = PlayerManager.Instance.PlayerData.AngularDrag;

            _currentLetterCount = PlayerManager.Instance.PlayerData.MaxLetters;

            return true;
        }

        [Client]
        private void InitializeLocalPlayer()
        {
            _viewer = (Camera.Viewer)ViewerManager.Instance.AcquireViewer();
            if(null != _viewer) {
                _viewer.Initialize(this);
            }

            UIManager.Instance.InitializePlayerUI(this);
            UIManager.Instance.PlayerUI.PlayerHUD.ShowInfoText();
        }

        [Server]
        public void Reload()
        {
            _reloadCompleteTime = TimeManager.Instance.CurrentUnixMs + PlayerManager.Instance.PlayerData.ReloadTimeMs;
            StartCoroutine(ReloadRoutine());
        }

        private IEnumerator ReloadRoutine()
        {
            yield return new WaitForSeconds(PlayerManager.Instance.PlayerData.ReloadTimeSeconds);

            _currentLetterCount = PlayerManager.Instance.PlayerData.MaxLetters;
        }

#region Commands
        [Command]
        public void CmdThrow(Vector3 origin, Vector3 direction, float speed)
        {
            if(!CanThrowMail) {
                return;
            }

            Mail mail = ItemManager.Instance.GetMail();
            mail?.Throw(this, origin, direction, speed);

            _currentLetterCount--;
            if(_currentLetterCount <= 0) {
                _currentLetterCount = 0;
                Reload();
            }
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
