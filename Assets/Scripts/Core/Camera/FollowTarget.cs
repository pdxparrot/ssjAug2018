using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
// TODO: make 2D vs 3D subclasses (probably need to abstract this)
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;

        public Vector3 LookAxis { get; set; }
    }
}
