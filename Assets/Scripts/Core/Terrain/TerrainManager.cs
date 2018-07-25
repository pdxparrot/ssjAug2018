using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Terrain
{
    public sealed class TerrainManager : SingletonBehavior<TerrainManager>
    {
        private readonly HashSet<TerrainHelper> _terrain = new HashSet<TerrainHelper>();

        public void RegisterTerrain(TerrainHelper terrain)
        {
            _terrain.Add(terrain);
        }

        public void UnregisterTerrain(TerrainHelper terrain)
        {
            _terrain.Remove(terrain);
        }
    }
}
