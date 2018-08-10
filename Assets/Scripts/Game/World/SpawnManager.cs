using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class SpawnManager : SingletonBehavior<SpawnManager>
    {
        private readonly HashSet<SpawnPoint> _spawnPoints = new HashSet<SpawnPoint>();

#region Registration
        public virtual void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            //Debug.Log($"Registering spawnpoint {spawnPoint.name}");

            _spawnPoints.Add(spawnPoint);
        }

        public virtual void UnregisterSpawnPoint(SpawnPoint spawnPoint)
        {
            //Debug.Log($"Unregistering spawnpoint {spawnPoint.name}");

            _spawnPoints.Remove(spawnPoint);
        }
#endregion

        [CanBeNull]
        public SpawnPoint GetSpawnPoint()
        {
            if(_spawnPoints.Count < 1) {
                Debug.LogWarning("No spawn points registered on spawn, are there any in the scene?");
                return null;
            }
            return PartyParrotManager.Instance.Random.GetRandomEntry(_spawnPoints);
        }
    }
}
