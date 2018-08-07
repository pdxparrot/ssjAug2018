using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(JumpControllerComponent))]
    [RequireComponent(typeof(DoubleJumpControllerComponent))]
    [RequireComponent(typeof(HoverControllerComponent))]
    [RequireComponent(typeof(ClimbingControllerComponent))]
    [RequireComponent(typeof(AimControllerComponent))]
    public sealed class PlayerController : CharacterActorController
    {
        [SerializeField]
        private PlayerControllerData _playerControllerData;

        [SerializeField]
        [ReadOnly]
        private float _fallStartHeight = float.MinValue;

        [Space(10)]

#region Throwing
        [Header("Throwing")]

        [SerializeField]
        private Transform _throwOrigin;

        [SerializeField]
        [ReadOnly]
        private bool _canThrowMail;

        [SerializeField]
        [ReadOnly]
        private Timer _autoThrowMailTimer;

        [SerializeField]
        [ReadOnly]
        private bool _canThrowSnowball;
#endregion

        public override bool CanMove => base.CanMove && !Player.IsStunned && !Player.IsDead;

        public Player Player => (Player)Owner;

#region Components
        public HoverControllerComponent HoverComponent { get; private set; }

        private ClimbingControllerComponent _climbingComponent;

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

            if(_climbingComponent.IsClimbing) {
                IsGrounded = true;
            }

            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowAimer(_aimComponent.IsAiming);
            }

            float dt = Time.deltaTime;

            _autoThrowMailTimer.Update(dt);
        }

        protected override void FixedUpdate()
        {
            bool wasFalling = IsFalling;

            base.FixedUpdate();

            if(!wasFalling && IsFalling) {
                _fallStartHeight = Rigidbody.position.y;
            } else if(wasFalling && !IsFalling && _playerControllerData.EnableFallStun) {
                float distance = _fallStartHeight - Rigidbody.position.y;
                if(distance >= _playerControllerData.FallStunDistance) {
                    Stun();
                }
                _fallStartHeight = float.MinValue;
            }

            Rigidbody.angularVelocity = Vector3.zero;
        }
#endregion

#region Actions
        public void StartThrowMail()
        {
            if(!CanMove || !Player.CanThrowMail) {
                return;
            }

            _canThrowMail = true;

            _autoThrowMailTimer.Start(_playerControllerData.AutoThrowSeconds, DoThrowMail);

            Player.Animator.SetBool(_playerControllerData.ThrowingMailParam, true);
        }

        public void ThrowMail()
        {
            if(!CanMove) {
                return;
            }

            if(_canThrowMail) {
                DoThrowMail();
            }

            Player.Animator.SetBool(_playerControllerData.ThrowingMailParam, false);

            _canThrowMail = true;
        }

        private void DoThrowMail()
        {
            _autoThrowMailTimer.Stop();

            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            Player.CmdThrowMail(_throwOrigin.position, /*!IsAiming ? Player.transform.forward :*/ Player.Viewer.transform.forward, _playerControllerData.ThrowSpeed);

            Player.Animator.SetTrigger(_playerControllerData.ThrowMailParam);
        }

        public void StartThrowSnowball()
        {
            if(!CanMove) {
                return;
            }

            _canThrowSnowball = true;

            //Player.Animator.SetBool(_playerControllerData.ThrowingSnowballParam, true);
        }

        public void ThrowSnowball()
        {
            if(!CanMove) {
                return;
            }

            if(_canThrowSnowball) {
                DoThrowSnowball();
            }

            //Player.Animator.SetBool(_playerControllerData.ThrowingSnowballParam, false);

            _canThrowSnowball = true;
        }

        private void DoThrowSnowball()
        {
            if(null == Player.Viewer) {
                Debug.LogWarning("Non-local player doing a throw!");
                return;
            }

            Player.CmdThrowSnowball(_throwOrigin.position, /*!IsAiming ? Player.transform.forward :*/ Player.Viewer.transform.forward, _playerControllerData.ThrowSpeed);

            //Player.Animator.SetTrigger(_playerControllerData.ThrowSnowballParam);
        }

        public override void ActionPerformed(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            if(action is JumpControllerComponent.JumpAction) {
                _climbingComponent.StopClimbing();
            }

            base.ActionPerformed(action);
        }
#endregion

        public void Stun()
        {
            if(IsAnimating) {
                return;
            }

            HoverComponent.StopHovering();
            _climbingComponent.StopClimbing();

            Player.Stun(_playerControllerData.FallStunTimeSeconds);
        }
    }
}
