using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Camera;
using pdxpartyparrot.ssjAug2018.GameState;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

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
        [Header("Inventory")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentLetterCount;

        public int CurrentLetterCount => _currentLetterCount;

        public bool CanThrowMail => CurrentLetterCount > 0;

        [SerializeField]
        [ReadOnly]
        //[SyncVar]
        private Timer _reloadTimer;

        public bool IsReloading => _reloadTimer.SecondsRemaining > 0.0f;
#endregion

        [Space(10)]

#region Score
        [Header("Score")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _score;

        public int Score => _score;
#endregion

        [Space(10)]

#region Stun
        [Header("Stun")]

        [SerializeField]
        [ReadOnly]
        //[SyncVar]
        private Timer _stunTimer;

        public bool IsStunned => _stunTimer.SecondsRemaining > 0.0f;
#endregion

        [Space(10)]

#region Dead
        [Header("Dead")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private bool _isDead;

        public bool IsDead => _isDead;

        [SerializeField]
        [ReadOnly]
        //[SyncVar]
        public Timer _respawnTimer;

        public bool IsAwaitingRespawn => IsDead && _respawnTimer.SecondsRemaining > 0.0f;
#endregion

        public FollowTarget FollowTarget { get; private set; }

        public PlayerController PlayerController => (PlayerController)Controller;

        public override float Height => ((CapsuleCollider)Collider).height;

        public override float Radius => ((CapsuleCollider)Collider).radius;

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

        private void Update()
        {
            float dt = Time.deltaTime;

            _reloadTimer.Update(dt);
            _stunTimer.Update(dt);
            _respawnTimer.Update(dt);
        }

        protected override void OnDestroy()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;

            if(PlayerManager.HasInstance) {
                PlayerManager.Instance.Unregister(this);
            }

            base.OnDestroy();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckMailboxTrigger(other.gameObject);
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
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowInfoText();
            }
        }

        [Server]
        private void CheckMailboxTrigger(GameObject go)
        {
            Mailbox mailbox = go.GetComponent<Mailbox>();
            if(null == mailbox) {
                return;
            }

            int consumed = mailbox.PlayerCollide(this);
            if(consumed < 1) {
                return;
            }

            _currentLetterCount -= consumed;
            CheckReload();
        }

        [Server]
        private void CheckReload()
        {
            if(_currentLetterCount > 0) {
                return;
            }

            _currentLetterCount = 0;
            Reload();
        }

        [Server]
        private void Reload()
        {
            _reloadTimer.Start(PlayerManager.Instance.PlayerData.ReloadTimeSeconds, () => {
                _currentLetterCount = PlayerManager.Instance.PlayerData.MaxLetters;
            });
        }

        [Server]
        public void IncreaseScore(int amount)
        {
            _score += amount;
        }

        [Server]
        public void Stun(float seconds)
        {
            if(IsStunned || IsDead) {
                return;
            }

            Debug.Log($"Player {name} is stunned for {seconds} seconds!");

            Controller.Rigidbody.velocity = new Vector3(0.0f, Controller.Rigidbody.velocity.y, 0.0f);

            Animator.SetBool(PlayerManager.Instance.PlayerData.StunnedParam, true);

            _stunTimer.Start(seconds, () => {
                Animator.SetBool(PlayerManager.Instance.PlayerData.StunnedParam, false);
            });
        }

        [Server]
        public void Kill()
        {
            if(IsDead) {
                return;
            }

            Debug.Log($"Player {name} is dead!");

            _isDead = true;
            Controller.Rigidbody.velocity = new Vector3(0.0f, Controller.Rigidbody.velocity.y, 0.0f);

            RpcDead();

            Animator.SetBool(PlayerManager.Instance.PlayerData.DeadParam, true);

            Debug.Log($"Player {name} respawning in {GameStateManager.Instance.GameData.PlayerRespawnSeconds} seconds");

            _respawnTimer.Start(GameStateManager.Instance.GameData.PlayerRespawnSeconds, () => {
                PlayerManager.Instance.RespawnPlayer(this);
            });
        }

#region Commands
        [Command]
        public void CmdThrowMail(Vector3 origin, Vector3 direction, float speed)
        {
            if(!CanThrowMail) {
                return;
            }

            Debug.Log($"Throwing mail from {origin} in direction {direction} at speed {speed}");

            Mail mail = ItemManager.Instance.GetMail();
            Vector3 velocity = direction * speed;
            if(null != mail) {
                mail.Throw(this, origin, velocity);
            }

            _currentLetterCount--;
            CheckReload();
        }

        [Command]
        public void CmdThrowSnowball(Vector3 origin, Vector3 direction, float speed)
        {
            Debug.Log($"TODO: throw snowball from {origin} in direction {direction} at speed {speed}");
        }
#endregion

#region Callbacks
        [ClientRpc]
        private void RpcDead()
        {
            Debug.Log("TODO: show player dead UI on client");
        }

        public override void OnSpawn()
        {
            Debug.Log($"Spawning player (isLocalPlayer={isLocalPlayer})");

            Initialize();

            _isDead = false;
        }

        public void OnRespawn()
        {
            Debug.Log($"Respawning player (isLocalPlayer={isLocalPlayer})");

            _isDead = false;

            Animator.SetBool(PlayerManager.Instance.PlayerData.DeadParam, false);
        }
#endregion
    }
}
