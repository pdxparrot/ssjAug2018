using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public interface IGrabbable
    {
        Collider Collider { get; }
    }
}
