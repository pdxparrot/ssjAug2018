using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
// TODO: make 2D vs 3D subclasses
    public abstract class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;

        public abstract Vector3 LookAxis { get; }
    }
}
