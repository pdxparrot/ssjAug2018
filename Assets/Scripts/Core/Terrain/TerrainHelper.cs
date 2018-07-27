using UnityEngine;

namespace pdxpartyparrot.Core.Terrain
{
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public sealed class TerrainHelper : MonoBehaviour
    {
#region Neighbors
        [Header("Neighbors")]

        [SerializeField]
        private UnityEngine.Terrain _left;

        [SerializeField]
        private UnityEngine.Terrain _right;

        [SerializeField]
        private UnityEngine.Terrain _top;

        [SerializeField]
        private UnityEngine.Terrain _bottom;
#endregion

        public UnityEngine.Terrain Terrain { get; private set; }

#region Unity Lifecycle
        private void Start()
        {
            Terrain = GetComponent<UnityEngine.Terrain>();
            Terrain.SetNeighbors(_left, _top, _right, _bottom);

            TerrainManager.Instance.RegisterTerrain(this);
        }

        private void OnDestroy()
        {
            if(TerrainManager.HasInstance) {
                TerrainManager.Instance.UnregisterTerrain(this);
            }
        }
#endregion
    }
}
