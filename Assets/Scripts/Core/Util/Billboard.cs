using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    public class Billboard : MonoBehaviour
    {
        [CanBeNull]
        public UnityEngine.Camera Camera { get; set; }

#region Unity Lifecycle
        private void LateUpdate()
        {
            if(null != Camera) {
                transform.forward = (Camera.transform.position - transform.position).normalized;
            }
        }
#endregion
    }
}
