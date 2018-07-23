using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.DeliveryTargets
{ 
    public class DeliveryTargetSpawnPoint : MonoBehaviour {

#region Unity Lifecycle
        protected virtual void Awake()
        {
            DeliveryTargetSpawnManager.Instance.RegisterDeliveryTargetSpawnPoint(this);
        }

        protected virtual void OnDestroy()
        {
            if (DeliveryTargetSpawnManager.HasInstance)
            {
                DeliveryTargetSpawnManager.Instance.UnregisterDeliveryTargetSpawnPoint(this);
            }
        }
#endregion

        public virtual void Spawn(IActor actor)
        {
            actor.GameObject.transform.position = transform.position;
            actor.GameObject.transform.rotation = transform.rotation;

            actor.GameObject.SetActive(true);
            actor.OnSpawn();

            // Self-Destruction unregisters the spawn point so it cannot be reused.
            Destroy(this);
        }
    }
}
