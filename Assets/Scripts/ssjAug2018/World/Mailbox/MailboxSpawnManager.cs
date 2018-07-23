using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;

using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    public class MailboxSpawnManager : SingletonBehavior<MailboxSpawnManager>
    {

        private readonly HashSet<MailboxSpawnPoint> _spawnPoints = new HashSet<MailboxSpawnPoint>();

#region Registration
        public virtual void RegisterDeliveryTargetSpawnPoint(MailboxSpawnPoint spawnPoint)
        {
            _spawnPoints.Add(spawnPoint);
        }

        public virtual void UnregisterDeliveryTargetSpawnPoint(MailboxSpawnPoint spawnPoint)
        {
            _spawnPoints.Remove(spawnPoint);
        }
#endregion

        public MailboxSpawnPoint GetFarSpawnPoint()
        {
            // TODO: Math. Get Player distance from spawn points, relative distance to spawn points.
            // Pick a 'Seed' Spawn as random mailbox within parameters, then choose 1 - 4 others within a radius of seed
            System.Random r = new System.Random();
            return r.GetRandomEntry(_spawnPoints);
        }

    }
}