using System;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Mailbox : MonoBehaviour, IComparable<Mailbox> {

#region Animations
        [Header("Animations")]

        [SerializeField]
        private Animator _animator;
#endregion
        
        private int _mailRequired = 0;
        private bool _isObjective = false;
        public bool isObjective => _isObjective;
        private int _timesActive = 0;

#region Unity Lifecycle
        private void Awake()
        {
            MailboxManager.Instance.RegisterMailbox(this);
        }

        private void OnDestroy()
        {
            if (MailboxManager.HasInstance)
            {
                MailboxManager.Instance.UnregisterMailbox(this);
            }
        }
#endregion

        // TODO: Add projectile onTriggernEnter logic, include playing audio and SFX (If present)
        // Also decrement the required deliveries
        // At 0, change to 'done' state (VFX applies)
        // Add UI for remaining delivers/total required

        public void ActivateMailbox(int requiredMail)
        {
            Debug.Log("HEY I ACTIVATED. I AM :" + this.name);
            _isObjective = true;
            _mailRequired = requiredMail;
            _timesActive++;
        }

        public void DeactivateMailbox()
        {
            _isObjective = false;
            MailboxManager.Instance.MailboxCompleted();
        }

        public void OnTriggerEnter(Collider other)
        {
            /* PLACEHOLDER FOR WHEN MAIL EXISTS
            if (other.GetComponent<Mail>().isActiveAndEnabled)
            {
                _mailRequired =- 1;
                if(_mailRequired == 0)
                {
                    _isObjective = false;
                } 
            }
            */
        }

        public int CompareTo(Mailbox other)
        {
            if (other == null) return 1;
            else
                return this._timesActive.CompareTo(other._timesActive);
        }
    }
}