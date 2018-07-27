using System;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Mailbox : MonoBehaviour {

#region Animations
        [Header("Animations")]

        [SerializeField]
        private Animator _animator;
#endregion
        
        private int _mailRequired = 0;
        private bool _isObjective = false;
        public bool isObjective => _isObjective;
        private bool _hasActivated = false;

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
            _isObjective = true;
            _mailRequired = requiredMail;
            _hasActivated = true;
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

        public static bool PreviouslyActivated(Mailbox box)
        {
            return box._hasActivated;
        }
    }
}