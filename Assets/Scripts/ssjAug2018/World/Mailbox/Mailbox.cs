using pdxpartyparrot.Core.Util;

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

#region Unity Lifecycle
        private void Awake()
        {
            MailboxManager.Instance.RegisterMailbox(this);

            DeactivateMailbox(false);
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

            _model.SetActive(false);
        }

        [Server]
        public void AddLetters(int count)
        {
            _mailRequired += count;
        }

        [Server]
        public void MailHit()
        {
            if(!_model.activeInHierarchy) {
                return;
            }

            Debug.Log($"Mailbox {name} hit by mail");

            _mailRequired--;
            if(_mailRequired == 0) {
                DeactivateMailbox();
            }
        }
    }
}