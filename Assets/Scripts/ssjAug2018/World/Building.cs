using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Building : MonoBehaviour, IGrabbable
    {
        private Collider _collider;

        public Collider Collider => _collider;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
#endregion
    }
}
