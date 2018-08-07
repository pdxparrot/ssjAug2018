using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    [RequireComponent(typeof(PlayerController))]
    public abstract class PlayerControllerComponent : CharacterActorControllerComponent
    {
        protected PlayerController PlayerController => (PlayerController)Controller;
    }
}
