using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;

using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.DeliveryTargets
{
    public class DeliveryTargetSpawnManager : SingletonBehavior<DeliveryTargetSpawnManager>
    {

        private readonly HashSet<DeliveryTargetSpawnPoint> _spawnPoints = new HashSet<DeliveryTargetSpawnPoint>();

#region Registration
        public virtual void RegisterDeliveryTargetSpawnPoint(DeliveryTargetSpawnPoint spawnPoint)
        {
            _spawnPoints.Add(spawnPoint);
        }

        public virtual void UnregisterDeliveryTargetSpawnPoint(DeliveryTargetSpawnPoint spawnPoint)
        {
            _spawnPoints.Remove(spawnPoint);
        }
#endregion

        public DeliveryTargetSpawnPoint GetFarSpawnPoint()
        {
            // TODO: Math. Get Player distance from spawn points, relative distance to spawn points.
            // Pick a 'Seed' Spawn as random mailbox within parameters, then choose 1 - 4 others within a radius of seed
            System.Random r = new System.Random();
            return r.GetRandomEntry(_spawnPoints);
        }

    }
}