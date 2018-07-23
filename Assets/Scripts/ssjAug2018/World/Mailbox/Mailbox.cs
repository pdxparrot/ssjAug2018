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
        private int _mailRequired =1;

        #region Unity Lifecycle
        private void Awake()
        {


        }

        private void OnDestroy()
        {

        }

        #endregion

        // TODO: Add projectile onCollisonEnter logic, include playing audio and SFX (If present)
        // Also decrement the required deliveries
        // At 0, change to 'done' state (VFX applies)
        // Add UI for remaining delivers/total required
    }
}