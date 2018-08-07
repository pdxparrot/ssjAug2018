using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class ThrowControllerComponent : PlayerControllerComponent
    {
        public class ThrowMailAction : CharacterActorControllerAction
        {
            public static ThrowMailAction Default = new ThrowMailAction();
        }

        public class ThrowSnowballAction : CharacterActorControllerAction
        {
            public static ThrowSnowballAction Default = new ThrowSnowballAction();
        }

        [SerializeField]
        [ReadOnly]
        private bool _canThrowMail;

        [SerializeField]
        [ReadOnly]
        private Timer _autoThrowMailTimer;

        [SerializeField]
        [ReadOnly]
        private bool _canThrowSnowball;

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            _autoThrowMailTimer.Update(dt);
        }
#endregion

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(action is ThrowMailAction) {
                _canThrowMail = true;

                _autoThrowMailTimer.Start(PlayerController.PlayerControllerData.AutoThrowSeconds, () => {
                    PlayerController.ThrowMail();
                    _canThrowMail = false;
                });

                PlayerController.Owner.Animator.SetBool(PlayerController.PlayerControllerData.ThrowingMailParam, true);
                return true;
            }

            if(action is ThrowSnowballAction) {
                _canThrowSnowball = true;

                //PlayerController.Owner.Animator.SetBool(PlayerController.PlayerControllerData.ThrowingSnowballParam, true);
                return true;
            }

            return false;
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(action is ThrowMailAction) {
                if(_canThrowMail) {
                    _autoThrowMailTimer.Stop();
                    PlayerController.ThrowMail();
                } else {
                    PlayerController.Owner.Animator.SetBool(PlayerController.PlayerControllerData.ThrowingMailParam, false);
                }

                _canThrowMail = false;
                return true;
            }

            if(action is ThrowSnowballAction) {
                if(_canThrowSnowball) {
                    PlayerController.ThrowSnowball();
                } else {
                    //PlayerController.Owner.Animator.SetBool(PlayerController.PlayerControllerData.ThrowingSnowballParam, false);
                }

                _canThrowSnowball = false;
                return true;
            }

            return false;
        }
    }
}
