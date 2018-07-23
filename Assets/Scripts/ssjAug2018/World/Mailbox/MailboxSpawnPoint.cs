using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{ 
    public class MailboxSpawnPoint : MonoBehaviour {

#region Unity Lifecycle
        protected virtual void Awake()
        {
            MailboxSpawnManager.Instance.RegisterDeliveryTargetSpawnPoint(this);
        }

        protected virtual void OnDestroy()
        {
            if (MailboxSpawnManager.HasInstance)
            {
                MailboxSpawnManager.Instance.UnregisterDeliveryTargetSpawnPoint(this);
            }
        }
#endregion

        public virtual void Spawn(IActor actor)
        {
            actor.GameObject.transform.position = transform.position;
            actor.GameObject.transform.rotation = transform.rotation;

            actor.GameObject.SetActive(true);
            actor.OnSpawn();
        }
    }
}
