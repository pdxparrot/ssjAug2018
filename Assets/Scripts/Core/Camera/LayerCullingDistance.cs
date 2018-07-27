using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class LayerCullingDistance : MonoBehaviour
    {
        // TODO: better to do a layer => distance mapping
        [SerializeField]
        private float[] _distances = new float[32];

#region Unity Lifecycle
        private void Start()
        {
            float[] actualDistances = new float[32];

            int minLength = System.Math.Min(actualDistances.Length, _distances.Length);
            for(int i=0; i<minLength; ++i) {
                actualDistances[i] = _distances[i];
            }
            GetComponent<UnityEngine.Camera>().layerCullDistances = actualDistances;
        }
#endregion
    }
}
