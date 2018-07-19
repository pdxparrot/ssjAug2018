using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public interface IActor
    {
        int Id { get; }

        GameObject GameObject { get; }

        GameObject Model { get; }

        ActorController Controller { get; }

        void Initialize(int id);

#region Callbacks
        void OnSpawn();
#endregion
    }
}
