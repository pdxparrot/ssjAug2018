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

        public void Initialize()
        {
            _viewer = ViewerManager.Instance.AcquireViewer();
_viewer.transform.position = new Vector3(0.0f, 5.0f, -15.0f);

            // TODO: acquire a gamepad
        }
    }
}
