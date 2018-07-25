using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(FollowCamera))]
    public abstract class FollowViewer : Viewer
    {
        private FollowCamera _followCamera;

        public FollowCamera FollowCamera => _followCamera;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _followCamera = GetComponent<FollowCamera>();
        }
#endregion

        public void SetMinOrbitRadius(float orbitMinRadius)
        {
            _followCamera.OrbitMinRadius = orbitMinRadius;
        }

        public void SetMaxOrbitRadius(float orbitMaxRadius)
        {
            _followCamera.OrbitMaxRadius = orbitMaxRadius;
        }
    }
}
