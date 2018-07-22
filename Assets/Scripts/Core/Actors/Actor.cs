using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public interface IActor
    {
        int Id { get; }

        GameObject GameObject { get; }

        GameObject Model { get; }

        Collider Collider { get; }

        ActorController Controller { get; }

        bool CanMove { get; }

        void Initialize(int id);

#region Callbacks
        void OnSpawn();
#endregion
    }
}
