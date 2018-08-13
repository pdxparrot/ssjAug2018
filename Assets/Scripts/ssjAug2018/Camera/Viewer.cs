using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Camera
{
    public sealed class Viewer : FollowViewer
    {
        private Player _target;

        private Transform _targetTransform;

        public void Initialize(Player target)
        {
            Initialize(0);

            Set3D();

            _target = target;
            _targetTransform = _target.FollowTarget.TargetTransform;

            FollowCamera.SetTarget(_target.FollowTarget);
            SetFocus(_target.transform);
        }

        public void Aim(Transform target)
        {
            _target.FollowTarget.TargetTransform = target;

            SetFocus(target);
        }

        public void ResetTarget()
        {
            _target.FollowTarget.TargetTransform = _targetTransform;

            SetFocus(_target.transform);
        }
    }
}
