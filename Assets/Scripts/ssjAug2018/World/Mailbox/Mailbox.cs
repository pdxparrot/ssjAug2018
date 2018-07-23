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
        
        [Space(10)]

        [SerializeField]
        private int _mailRequired = 0;

        private bool _isObjective = false;

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

        // TODO: Add projectile onCollisonEnter logic, include playing audio and SFX (If present)
        // Also decrement the required deliveries
        // At 0, change to 'done' state (VFX applies)
        // Add UI for remaining delivers/total required

        public void ActivateObjective(int requiredMail)
        {
            _isObjective = true;
            _mailRequired = requiredMail;
            _timesActive++;
        }
    }
}