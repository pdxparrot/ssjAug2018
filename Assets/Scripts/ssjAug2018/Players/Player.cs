using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class Player : NetworkActor
    {
        private Viewer _viewer;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkIdentity.localPlayerAuthority = true;

            PlayerManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;

            if(PlayerManager.HasInstance) {
                PlayerManager.Instance.Unregister(this);
            }
        }
#endregion

        public bool Initialize()
        {
            _viewer = ViewerManager.Instance.AcquireViewer();
            if(null == _viewer) {
                return false;
            }

            _viewer.Set3D();
_viewer.transform.position = new Vector3(0.0f, 5.0f, -10.0f);

            Controller.Initialize(this);

            return true;
        }

#region Callbacks
        public override void OnSpawn()
        {
        }
#endregion
    }
}
