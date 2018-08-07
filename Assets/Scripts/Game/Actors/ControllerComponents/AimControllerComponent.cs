using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class AimControllerComponent : CharacterActorControllerComponent
    {
        public class AimAction : CharacterActorControllerAction
        {
            public static AimAction Default = new AimAction();
        }

        [SerializeField]
        [ReadOnly]
        private bool _isAiming;

        public bool IsAiming => _isAiming;

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = true;

            Debug.Log("TODO: zoom and aim!");

            return true;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = false;

            return true;
        }
    }
}
