using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorDriver : MonoBehaviour
    {
        protected ActorController Controller { get; private set; }

        protected IActor Owner { get; private set; }

        public void Initialize(IActor owner, ActorController controller)
        {
            Owner = owner;
            Controller = controller;
        }
    }
}
