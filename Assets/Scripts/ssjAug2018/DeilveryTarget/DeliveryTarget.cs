using pdxpartyparrot.Core.Actors;

using UnityEngine;

// TODO: Lots. Basic class implementation so I can reference this in the manager
namespace pdxpartyparrot.ssjAug2018.DeliveryTargets
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class DeliveryTarget : NetworkActor {

        /* Unsure if this is going to get animations.
        #region Animations
        [Header("Animations")]

        [SerializeField]
        private Animator _animator;
        #endregion
        */

        public int DeliveriesRequired;

        private AudioSource _audiosource;

        #region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkIdentity.localPlayerAuthority = false;

            _audiosource = GetComponent<AudioSource>();
            _audiosource.playOnAwake = false;


        }

        private void OnDestroy()
        {
            _audiosource.Stop();
        }

        #endregion

        // TODO: Add projectile onCollisonEnter logic, include playing audio and SFX (If present)
        // Also decrement the required deliveries
        // At 0, change to 'done' state (VFX applies)
        // Add UI for remaining delivers/total required

        public override void OnSpawn() {
            Debug.Log("Spawned " + this.name);
        }
    }
}