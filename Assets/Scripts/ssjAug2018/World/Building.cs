using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public class Building : MonoBehaviour
    {
        private Collider _collider;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
#endregion
    }
}
