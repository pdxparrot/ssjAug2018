using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ssjAug2018.Players;

namespace pdxpartyparrot.ssjAug2018.Camera
{
    public class Viewer : FollowViewer
    {
        public void Initialize(Player owner)
        {
            base.Initialize(0);

            Set3D();

            FollowCamera.SetTarget(owner.FollowTarget);
            //SetOrbitRadius(...);

            //SetGlobalPostProcessProfile(...);
        }
    }
}
