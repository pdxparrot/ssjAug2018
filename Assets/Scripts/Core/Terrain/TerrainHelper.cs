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

        private UnityEngine.Terrain _terrain;

#region Unity Lifecycle
        private void Awake()
        {
            _terrain = GetComponent<UnityEngine.Terrain>();
            _terrain.SetNeighbors(_left, _top, _right, _bottom);

            TerrainManager.Instance.RegisterTerrain(this);
        }

        private void OnDestroy()
        {
            TerrainManager.Instance.UnregisterTerrain(this);
        }
#endregion
    }
}
