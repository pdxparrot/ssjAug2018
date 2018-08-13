using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Audio;
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

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private bool _hasActivated;

        public bool HasActivated => _hasActivated;

        [Space(10)]
        [Header("Effects")]
        [SerializeField]
        private EffectTrigger _receivedMailAudio;
       
        [SerializeField]
        private EffectTrigger _mailboxCompleteAudio;

#region Unity Lifecycle
        private void Awake()
        {
            MailboxManager.Instance.RegisterMailbox(this);

            if(NetworkServer.active) {
                DeactivateMailbox(false);
            }
        }

        private void OnDestroy()
        {
            if(MailboxManager.HasInstance) {
                MailboxManager.Instance.UnregisterMailbox(this);
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
        public void DeactivateMailbox(bool complete=true)
        {
            if(complete) {
                Debug.Log($"Mailbox {name} completed");
                MailboxManager.Instance.MailboxCompleted(this);
            }
            _mailboxCompleteAudio.Trigger();
            _model.SetActive(false);
        }

        [Server]
        public void ForceComplete()
        {
            _mailRequired = 0;
            DeactivateMailbox();
        }

        [Server]
        public bool MailHit(Player owner)
        {
            if(!_model.activeInHierarchy) {
                return false;
            }

            Debug.Log($"Mailbox {name} hit by mail");

            _mailRequired--;
            if(_mailRequired <= 0) {
                DeactivateMailbox(owner);
            }

            _receivedMailAudio.Trigger();
            GameManager.Instance.ScoreHit(owner);
            return true;
        }

        [Server]
        public int PlayerCollide(Player player)
        {
            if(!_model.activeInHierarchy || !GameStateManager.Instance.GameData.PlayerCollidesMailboxes) {
                return -1;
            }

            Debug.Log($"Mailbox {name} hit by player");

            int consumed = player.NetworkPlayer.CurrentLetterCount < _mailRequired ? player.NetworkPlayer.CurrentLetterCount : _mailRequired;

            _mailRequired -= consumed;
            if(_mailRequired <= 0) {
                DeactivateMailbox(player);
            }

            GameManager.Instance.Score(player);

            return consumed;
        }
    }
}