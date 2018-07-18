using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerController : ActorController
    {
#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitRigidbody();
        }
#endregion

        private void InitRigidbody()
        {
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            Rigidbody.freezeRotation = true;
            Rigidbody.detectCollisions = true;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            Rigidbody.interpolation = RigidbodyInterpolation.None;
        }
    }
}
