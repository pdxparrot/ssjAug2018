using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorDriver : MonoBehaviour
    {
        [SerializeField]
        private ActorController _controller;

        protected ActorController Controller => _controller;

        protected virtual bool CanDrive => !PartyParrotManager.Instance.IsPaused;
    }
}
