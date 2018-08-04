using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    [RequireComponent(typeof(CharacterActorController))]
    public abstract class CharacterActorControllerComponent : MonoBehaviour
    {
        protected CharacterActorController Controller { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Controller = GetComponent<CharacterActorController>();
        }
#endregion

        public virtual bool OnJump()
        {
            return false;
        }
    }
}
