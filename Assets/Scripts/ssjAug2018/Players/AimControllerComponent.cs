using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players.ControllerComponents
{
    public sealed class AimControllerComponent : PlayerControllerComponent
    {
        public class AimAction : CharacterActorControllerAction
        {
            public static AimAction Default = new AimAction();
        }

        [SerializeField]
        private Transform _aimFollowTarget;

        [SerializeField]
        private Transform _aimFocusTarget;

        [SerializeField]
        [ReadOnly]
        private bool _isAiming;

        public bool IsAiming => _isAiming;

#region Unity Lifecycle
        private void Update()
        {
            if(IsAiming) {
                Controller.LastMoveAxes = Vector3.zero;
            }
        }
#endregion

        public override bool OnAnimationMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            if(null == PlayerController.ClimbingComponent || !PlayerController.ClimbingComponent.IsClimbing) {
                Vector3 viewerForward = null != PlayerController.Player.Viewer
                                        ? PlayerController.Player.Viewer.transform.forward
                                        : PlayerController.Player.transform.forward;
                PlayerController.Player.transform.forward = new Vector3(viewerForward.x, 0.0f, viewerForward.z).normalized;
            }

            return true;
        }

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            // stop our directional movement
            Vector3 velocity = Controller.Rigidbody.velocity;
            velocity.x = velocity.z = 0.0f;
            Controller.Rigidbody.velocity = velocity;

            return true;
        }

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = true;

            PlayerController.Player.PlayerViewer.Aim(_aimFollowTarget, _aimFocusTarget);

            return true;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            PlayerController.Player.PlayerViewer.ResetTarget();

            _isAiming = false;

            return true;
        }
    }
}
