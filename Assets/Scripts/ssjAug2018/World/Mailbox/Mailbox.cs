using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.ssjAug2018.GameState;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(Collider))]
    public sealed class Mailbox : NetworkBehaviour
    {
        [SerializeField]
        private GameObject _model;

        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _mailRequired;

        public bool IsCompleted => _mailRequired <= 0;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private bool _hasActivated;

        public bool HasActivated => _hasActivated;

        [Space(10)]

#region Effects
        [Header("Effects")]

        [SerializeField]
        private EffectTrigger _receivedMail;
       
        [SerializeField]
        private EffectTrigger _mailboxComplete;
#endregion

        [SerializeField]
        [ReadOnly]
        private Timer _despawnTimer;

#region Unity Lifecycle
        private void Awake()
        {
            MailboxManager.Instance.RegisterMailbox(this);

            if(NetworkServer.active) {
                _model.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if(MailboxManager.HasInstance) {
                MailboxManager.Instance.UnregisterMailbox(this);
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if(NetworkServer.active) {
                _despawnTimer.Update(dt);
            }
        }
#endregion

        [Server]
        public void Reset()
        {
            _hasActivated = false;
        }

        [Server]
        public void ActivateMailbox(int requiredMail)
        {
            _mailRequired = requiredMail;
            _hasActivated = true;

            _model.SetActive(true);

            Debug.Log($"Mailbox {name} activated requiring {_mailRequired} mail");
        }

        [Server]
        public void Complete()
        {
            Debug.Log($"Mailbox {name} completed");
            _mailRequired = 0;

            RpcComplete();

            _despawnTimer.Start(MailboxManager.Instance.MailboxData.CompletedMailboxDespawnSeconds, () => {
                MailboxManager.Instance.MailboxCompleted(this);
                _model.SetActive(false);
            });
        }

        [Server]
        public bool MailHit(Player owner)
        {
            if(IsCompleted || !_model.activeInHierarchy) {
                return false;
            }

            Debug.Log($"Mailbox {name} hit by mail");

            _mailRequired--;
            if(_mailRequired <= 0) {
                Complete();
            } else {
                RpcHit();
            }

            GameManager.Instance.ScoreHit(owner);
            return true;
        }

        [Server]
        public int PlayerCollide(Player player)
        {
            if(IsCompleted || !_model.activeInHierarchy || !GameStateManager.Instance.GameData.PlayerCollidesMailboxes) {
                return -1;
            }

            Debug.Log($"Mailbox {name} hit by player");

            int consumed = player.NetworkPlayer.CurrentLetterCount < _mailRequired ? player.NetworkPlayer.CurrentLetterCount : _mailRequired;

            _mailRequired -= consumed;
            if(_mailRequired <= 0) {
                Complete();
            } else {
                RpcHit();
            }

            GameManager.Instance.Score(player, consumed);
            return consumed;
        }

#region Callbacks
        [ClientRpc]
        private void RpcComplete()
        {
            _mailboxComplete.Trigger();
        }

        [ClientRpc]
        private void RpcHit()
        {
            _receivedMail.Trigger();
        }
#endregion
    }
}