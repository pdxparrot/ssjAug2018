using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Camera
{
// TODO: rename PlayerViewer
    public sealed class Viewer : FollowViewer
    {
        private Player _target;

        private Transform _targetTransform;

        private float _maxOrbit;

        public void Initialize(Player target)
        {
            Initialize(0);

            Set3D();

            _target = target;
            _targetTransform = _target.FollowTarget.TargetTransform;

            FollowCamera.SetTarget(_target.FollowTarget);
            SetFocus(_target.transform);
        }

        public void Aim(Transform target, Transform focusTarget)
        {
            _target.FollowTarget.TargetTransform = target;

            _maxOrbit = FollowCamera.OrbitMaxRadius;
            FollowCamera.OrbitMaxRadius = 1.0f;

            SetFocus(focusTarget);
        }

        public void ResetTarget()
        {
            _target.FollowTarget.TargetTransform = _targetTransform;

            FollowCamera.OrbitMaxRadius = _maxOrbit;

            SetFocus(_target.transform);
        }
    }
}
