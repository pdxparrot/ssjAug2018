using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.GameState;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(NetworkAnimator))]
    public sealed class NetworkPlayer : NetworkActor
    {
#region Inventory
        [Header("Inventory")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentLetterCount;

        public int CurrentLetterCount => _currentLetterCount;

        [SerializeField]
        [ReadOnly]
        private Timer _reloadTimer;

        public bool IsReloading => _reloadTimer.SecondsRemaining > 0.0f;

       public bool CanThrowMail => !IsReloading && CurrentLetterCount > 0;
#endregion

#region Score
        [Header("Score")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _score;

        public int Score => _score;
#endregion

#region Stun
        [SerializeField]
        [ReadOnly]
        private Timer _stunTimer;

        public bool IsStunned => _stunTimer.SecondsRemaining > 0.0f;
#endregion

#region Dead
        [Header("Dead")]

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private bool _isDead;

        public bool IsDead => _isDead;

        [SerializeField]
        [ReadOnly]
        private Timer _respawnTimer;

        public bool IsAwaitingRespawn => IsDead && _respawnTimer.SecondsRemaining > 0.0f;
#endregion

        public Player Player => (Player)Actor;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkIdentity.localPlayerAuthority = true;
            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if(NetworkServer.active) {
                _reloadTimer.Update(dt);
                _stunTimer.Update(dt);
                _respawnTimer.Update(dt);
            }
        }
#endregion

        [Server]
        public void Reset()
        {
            Player.Controller.Rigidbody.mass = PlayerManager.Instance.PlayerData.Mass;
            Player.Controller.Rigidbody.drag = PlayerManager.Instance.PlayerData.Drag;
            Player.Controller.Rigidbody.angularDrag = PlayerManager.Instance.PlayerData.AngularDrag;

            _currentLetterCount = PlayerManager.Instance.PlayerData.MaxLetters;
            _isDead = false;
        }

        [Server]
        public void CheckMailboxTrigger(GameObject go)
        {
            Mailbox mailbox = go.GetComponent<Mailbox>();
            if(null == mailbox) {
                return;
            }

            int consumed = mailbox.PlayerCollide(Player);
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

            Player.Controller.Rigidbody.velocity = new Vector3(0.0f, Player.Controller.Rigidbody.velocity.y, 0.0f);

            Player.Animator.SetBool(PlayerManager.Instance.PlayerData.StunnedParam, true);

            _stunTimer.Start(seconds, () => {
                Player.Animator.SetBool(PlayerManager.Instance.PlayerData.StunnedParam, false);
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
            Player.Controller.Rigidbody.velocity = new Vector3(0.0f, Player.Controller.Rigidbody.velocity.y, 0.0f);

            RpcDead();

            Player.Animator.SetBool(PlayerManager.Instance.PlayerData.DeadParam, true);

            Debug.Log($"Player {name} respawning in {GameStateManager.Instance.GameData.PlayerRespawnSeconds} seconds");

            _respawnTimer.Start(GameStateManager.Instance.GameData.PlayerRespawnSeconds, () => {
                PlayerManager.Instance.RespawnPlayer(Player);
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
                mail.Throw(Player, origin, velocity);
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

// TODO: we could make better use of NetworkBehaviour callbacks in here (and in other NetworkBehaviour types)

#region Callbacks
        [ClientRpc]
        public void RpcSpawn(int id)
        {
            Player.Initialize(id);
        }

        [ClientRpc]
        public void RpcGameTimeUpdated(int amount)
        {
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowTimeAdded(amount);
            }
        }

        [ClientRpc]
        public void RpcHit()
        {
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowHitMarker();
            }
        }

        [ClientRpc]
        private void RpcDead()
        {
            Debug.Log("TODO: show player dead UI on client");
        }
#endregion
    }
}
