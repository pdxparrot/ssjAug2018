using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Players.ControllerComponents;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerController : CharacterActorController
    {
        [SerializeField]
        private PlayerControllerData _playerControllerData;

        public PlayerControllerData PlayerControllerData => _playerControllerData;

        [SerializeField]
        [ReadOnly]
        private float _fallStartHeight = float.MinValue;

        [SerializeField]
        private Transform _throwOrigin;

        public override bool CanMove => base.CanMove && !Player.NetworkPlayer.IsStunned && !Player.NetworkPlayer.IsDead;

        public Player Player => (Player)Owner;

        public bool IsAiming => null != _aimComponent && _aimComponent.IsAiming;

        private Vector3 ThrowDirection
        {
            get
            {
                if(null == Player.Viewer || IsMoving) {
                    return Player.transform.forward;
                }

                /*Vector3 target = Player.Viewer.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)).direction * PlayerControllerData.ThrowConvergeDistance;
                return (target - _throwOrigin.position).normalized;*/
                return Player.Viewer.transform.forward;
            }
        }

#region Components
        [CanBeNull]
        public HoverControllerComponent HoverComponent { get; private set; }

        [CanBeNull]
        private ClimbingControllerComponent _climbingComponent;

        [CanBeNull]
        private AimControllerComponent _aimComponent;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            HoverComponent = GetControllerComponent<HoverControllerComponent>();
            _climbingComponent = GetControllerComponent<ClimbingControllerComponent>();
            _aimComponent = GetControllerComponent<AimControllerComponent>();
        }

        protected override void Update()
        {
            base.Update();

            if(null != _climbingComponent && _climbingComponent.IsClimbing) {
                IsGrounded = true;
            }

            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowAimer(null != _aimComponent && _aimComponent.IsAiming);
            }
        }

        protected override void FixedUpdate()
        {
            bool wasFalling = IsFalling;

            base.FixedUpdate();

            if(!wasFalling && IsFalling) {
                _fallStartHeight = Rigidbody.position.y;
            } else if(wasFalling && !IsFalling && PlayerControllerData.EnableFallStun) {
                float distance = _fallStartHeight - Rigidbody.position.y;
                if(distance >= PlayerControllerData.FallStunDistance) {
                    Stun();
                }
                _fallStartHeight = float.MinValue;
            }

            Rigidbody.angularVelocity = Vector3.zero;
        }
#endregion

        public override void ActionPerformed(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            if(action is JumpControllerComponent.JumpAction && null != _climbingComponent) {
                _climbingComponent.StopClimbing();
            }

            base.ActionPerformed(action);
        }

        public void ThrowMail()
        {
            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            if(!IsMoving) {
                Player.transform.forward = new Vector3(ThrowDirection.x, 0.0f, ThrowDirection.z).normalized;
            }
            Player.NetworkPlayer.CmdThrowMail(_throwOrigin.position, ThrowDirection, PlayerControllerData.ThrowSpeed);

            Player.Animator.SetTrigger(PlayerControllerData.ThrowMailParam);
            Player.Animator.SetBool(PlayerControllerData.ThrowingMailParam, false);
        }

        public void ThrowSnowball()
        {
            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            Player.NetworkPlayer.CmdThrowSnowball(_throwOrigin.position, ThrowDirection, PlayerControllerData.ThrowSpeed);

            Player.Animator.SetTrigger(PlayerControllerData.ThrowSnowballParam);
            Player.Animator.SetBool(PlayerControllerData.ThrowingSnowballParam, false);
        }

        public void Stun()
        {
            if(IsAnimating) {
                return;
            }

            if(null != HoverComponent) {
                HoverComponent.StopHovering();
            }

            if(null != _climbingComponent) {
                _climbingComponent.StopClimbing();
            }

            Player.NetworkPlayer.Stun(PlayerControllerData.FallStunTimeSeconds);
        }
    }
}
