using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Building : MonoBehaviour, IGrabbable
    {
        public Collider Collider { get; private set; }

#region Unity Lifecycle
        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }
#endregion
    }
}
