using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Rendering;
using pdxpartyparrot.Core.UI;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace pdxpartyparrot.Core.Camera
{
    public abstract class Viewer : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _id;

        public int Id => _id;

        [Space(10)]

#region Cameras
        [Header("Cameras")]

        [SerializeField]
        private UnityEngine.Camera _camera;

        public UnityEngine.Camera Camera => _camera;

        [SerializeField]
        private UnityEngine.Camera _uiCamera;

        public UnityEngine.Camera UICamera => _uiCamera;
#endregion

        [Space(10)]

#region Post Processing
        [Header("Post Processing")]

        [SerializeField]
        private PostProcessVolume _globalPostProcessVolume;

        public PostProcessProfile GlobalPostProcessProfile { get; private set; }
#endregion

        [SerializeField]
        [ReadOnly]
        private Vector3 _defaultCameraPosition;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            _defaultCameraPosition = Camera.transform.localPosition;

            _uiCamera.clearFlags = CameraClearFlags.Nothing;
            _uiCamera.backgroundColor = Color.black;
            _uiCamera.orthographic = true;
            _uiCamera.cullingMask = UIManager.Instance.UILayer;
            _uiCamera.useOcclusionCulling = false;

            _globalPostProcessVolume.isGlobal = true;
            _globalPostProcessVolume.priority = 1;
        }

        protected virtual void OnDestroy()
        {
            Reset();
        }
#endregion

        public virtual void Initialize(int id)
        {
            _id = id;

            name = $"Viewer {Id}";

            // setup the camera to only render it's own post processing volume
            LayerMask postProcessLayer = LayerMask.NameToLayer($"Viewer{Id}_PostProcess");
            if(postProcessLayer != -1) {
                _globalPostProcessVolume.gameObject.layer = postProcessLayer;

                PostProcessLayer layer = Camera.GetComponent<PostProcessLayer>();
                layer.volumeLayer = 1 << postProcessLayer.value;
            }
        }

#region Orthographic/Perspective
        public void Set2D()
        {
            Camera.orthographic = true;
        }

        public void Set2D(float size)
        {
            Set2D();
            Camera.orthographicSize = size;
        }

        public void Set3D()
        {
            Camera.orthographic = false;
        }

        public void Set3D(float fieldOfView)
        {
            Set3D();
            Camera.fieldOfView = fieldOfView;
        }
#endregion

        public void EnableCamera(bool enable)
        {
            Camera.enabled = enable;
        }

        public void EnableUICamera(bool enable)
        {
            UICamera.enabled = enable;
        }

        public void ResetCameraPosition()
        {
            Camera.transform.localPosition = _defaultCameraPosition;
        }

        public virtual void Reset()
        {
            ResetCameraPosition();

            ResetPostProcessProfile();
        }

        private void ResetPostProcessProfile()
        {
            GlobalPostProcessProfile?.Destroy();
            GlobalPostProcessProfile = null;

            _globalPostProcessVolume.profile = null;
        }

        public void SetViewport(int x, int y, float viewportWidth, float viewportHeight)
        {
            float viewportX = x * viewportWidth;
            float viewportY = y * viewportHeight;

            Rect viewport = new Rect(
                viewportX + ViewerManager.Instance.ViewportEpsilon,
                viewportY + ViewerManager.Instance.ViewportEpsilon,
                viewportWidth - (ViewerManager.Instance.ViewportEpsilon * 2),
                viewportHeight - (ViewerManager.Instance.ViewportEpsilon * 2));

            Camera.rect = viewport;
            UICamera.rect = viewport;

            AspectRatio aspectRatio = UICamera.GetComponent<AspectRatio>();
            if(null != aspectRatio) {
                aspectRatio.UpdateAspectRatio();
            }
        }

        public void SetGlobalPostProcessProfile(PostProcessProfile profile)
        {
            ResetPostProcessProfile();

            GlobalPostProcessProfile = profile;
            _globalPostProcessVolume.profile = profile;
        }

#region Render Layers
        public void AddRenderLayer(LayerMask layer)
        {
            Camera.cullingMask |= (1 << layer.value);
        }

        public void RemoveRenderLayer(LayerMask layer)
        {
            Camera.cullingMask &= ~(1 << layer.value);
        }
#endregion
    }
}
