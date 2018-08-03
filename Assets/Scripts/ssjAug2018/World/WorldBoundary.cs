using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public class WorldBoundary : MonoBehaviour
    {
        [SerializeField]
        private bool _deadly;

#region Unity Lifecycle
        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleCollision(other.gameObject);
        }
#endregion

        private void HandleCollision(GameObject go)
        {
            Player player = go.GetComponent<Player>();
            if(null == player) {
                return;
            }

            if(_deadly) {
                player.Kill();
            }
        }
    }
}
