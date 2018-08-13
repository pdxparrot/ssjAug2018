using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ssjAug2018.Actors;

namespace pdxpartyparrot.ssjAug2018.Camera
{
    public sealed class ServerSpectatorViewer : FollowViewer
    {
        public void Initialize(ServerSpectator owner)
        {
            Initialize(0);

            Set3D();

            FollowCamera.SetTarget(owner.FollowTarget);
            SetFocus(owner.transform);
        }
    }
}
