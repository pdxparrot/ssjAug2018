﻿using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public interface IActor
    {
        int Id { get; }

        GameObject GameObject { get; }

        GameObject Model { get; }

        Collider Collider { get; }

        Animator Animator { get; }

        ActorController Controller { get; }

        [CanBeNull]
        Viewer Viewer { get; }

        void Initialize(int id);

#region Callbacks
        void OnSpawn();
#endregion
    }
}
