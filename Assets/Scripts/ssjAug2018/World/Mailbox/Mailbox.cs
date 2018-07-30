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

        private Collider _collider;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<Collider>();

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

        public void Reset()
        {
            _hasActivated = false;
        }

        public void ActivateMailbox(int requiredMail)
        {
            _mailRequired = requiredMail;
            _hasActivated = true;

            _model.SetActive(true);
            _collider.enabled = true;

            Debug.Log($"Mailbox {name} activated requiring {_mailRequired} mail");
        }

        public void DeactivateMailbox(bool complete=true)
        {
            if(complete) {
                Debug.Log($"Mailbox {name} completed");
                MailboxManager.Instance.MailboxCompleted(this);
            }

            _model.SetActive(false);
            _collider.enabled = false;
        }

        public void AddLetters(int count)
        {
            _mailRequired += count;
        }

        public void MailHit()
        {
            Debug.Log($"Mailbox {name} hit by mail");

            _mailRequired--;
            if(_mailRequired == 0) {
                DeactivateMailbox();
            }
        }
    }
}