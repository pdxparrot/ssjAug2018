using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public interface IFollowTarget
    {
        GameObject GameObject { get; }

        Collider Collider { get; }

        Vector3 LookAxis { get; }

        bool IsPaused { get; }
    }
}
