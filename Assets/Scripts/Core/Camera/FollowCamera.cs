using pdxpartyparrot.Core.Math;
using pdxpartyparrot.Core.Util;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(Collider))]
    public sealed class FollowCamera : MonoBehaviour
    {
#region Orbit Config
        [Header("Orbit")]

        [SerializeField]
        [Tooltip("Enable looking around the follow target")]
        private bool _enableOrbit = true;

        [SerializeField]
        [Range(0, 100)]
        private float _orbitSpeedX = 100.0f;

        [SerializeField]
        [Range(0, 100)]
        private float _orbitSpeedY = 100.0f;

        [SerializeField]
        [Tooltip("Return to the default orbit rotation when released")]
        private bool _returnToDefault;

        [SerializeField]
        private Vector2 _defaultOrbitRotation = new Vector2(0.0f, 30.0f);

        [SerializeField]
        [Range(0, 1)]
        private float _defaultOrbitReturnTime = 0.5f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _orbitReturnVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector2 _orbitRotation;

        // TODO: use this
        [SerializeField]
        [Range(0, 50)]
        [Tooltip("The minimum distance the camera should be from the follow target")]
        private float _orbitMinRadius = 15.0f;

        public float OrbitMinRadius { get { return _orbitMinRadius; } set { _orbitMinRadius = value; } }

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("The maximum distance the camera should be from the follow target")]
        private float _orbitMaxRadius = 15.0f;

        public float OrbitMaxRadius { get { return _orbitMaxRadius; } set { _orbitMaxRadius = value; } }
#endregion

#region Orbit Constraints
        [SerializeField]
        [Range(-360, 0)]
        private float _orbitXMin = -90.0f;

        [SerializeField]
        [Range(0, 360)]
        private float _orbitXMax = 90.0f;

        [SerializeField]
        [Range(-360, 0)]
        private float _orbitYMin = -90.0f;

        [SerializeField]
        [Range(0, 360)]
        private float _orbitYMax = 90.0f;
#endregion

        [Space(10)]

#region Zoom Config
        [Header("Zoom")]

        [SerializeField]
        [Tooltip("Enable zooming in and out relative to the follow target")]
        private bool _enableZoom = false;

        [SerializeField]
        [Range(0, 10)]
        private float _minZoomDistance = 5.0f;

        [SerializeField]
        [Range(0, 100)]
        private float _maxZoomDistance = 100.0f;

        [SerializeField]
        [Range(0, 500)]
        private float _zoomSpeed = 500.0f;
#endregion

        [Space(10)]

#region Look Config
        [Header("Look")]

        [SerializeField]
        [Tooltip("Enable rotating the camera around its local axes")]
        private bool _enableLook = false;

        [SerializeField]
        [Range(0, 100)]
        private float _lookSpeedX = 100.0f;

        [SerializeField]
        [Range(0, 100)]
        private float _lookSpeedY = 100.0f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _lookRotation;
#endregion

        [Space(10)]

#region Target
        [Header("Target")]

        [SerializeField]
        [Tooltip("The target to follow")]
        [CanBeNull]
        private FollowTarget _target;

        [CanBeNull]
        public FollowTarget Target => _target;
#endregion

        [Space(10)]

#region Smooth
        [SerializeField]
        [Tooltip("Smooth the camera movement as it follows the target")]
        private bool _smooth;

        [SerializeField]
        [Range(0, 0.5f)]
        private float _smoothTime = 0.05f;

        [SerializeField]
        [ReadOnly]
        private Vector3 _velocity;
#endregion

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastTargetPosition;

        [SerializeField]
        [ReadOnly]
        private bool _isLooking;

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            HandleInput(dt);
        }

        private void LateUpdate()
        {
            if(_smooth) {
                return;
            }

            float dt = Time.deltaTime;

            FollowTarget(dt);
        }

        private void FixedUpdate()
        {
            if(!_smooth) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            FollowTarget(dt);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("TODO: FollowCamera collision!");
        }
#endregion

        public void SetTarget(FollowTarget target)
        {
            _target = target;
            _orbitRotation = _defaultOrbitRotation;
        }

        private void HandleInput(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera.HandleInput");
            try {
                bool wasLooking = _isLooking;
                _isLooking = Target.LastLookAxes.sqrMagnitude >= float.Epsilon;
                if(!wasLooking && !_isLooking) {
                    return;
                }

                Orbit(Target.LastLookAxes, dt);
                Zoom(Target.LastLookAxes, dt);
                Look(Target.LastLookAxes, dt);
            } finally {
                Profiler.EndSample();
            }
        }

        private void Orbit(Vector3 axes, float dt)
        {
            if(!_enableOrbit) {
                return;
            }

            // TODO: this is fighting too hard at max rotation
            // (or maybe the max rotation clamping is killing it)
            if(_returnToDefault) {
                _orbitRotation = Vector2.SmoothDamp(_orbitRotation, _defaultOrbitRotation, ref _orbitReturnVelocity, _defaultOrbitReturnTime, Mathf.Infinity, dt);
            }

            _orbitRotation.x = Mathf.Clamp(MathUtil.WrapAngle(_orbitRotation.x + axes.x * _orbitSpeedX * dt), _orbitXMin, _orbitXMax);
            _orbitRotation.y = Mathf.Clamp(MathUtil.WrapAngle(_orbitRotation.y - axes.y * _orbitSpeedY * dt), _orbitYMin, _orbitYMax);
        }

        private void Zoom(Vector3 axes, float dt)
        {
            if(!_enableZoom) {
                return;
            }

            float zoomAmount = axes.z * _zoomSpeed * dt;

            float minDistance = _minZoomDistance, maxDistance = _maxZoomDistance;
            if(null != Target) {
                // avoid zooming into the target
                Vector3 closestBoundsPoint = Target.Collider.ClosestPointOnBounds(transform.position);
                float distanceToPoint = (closestBoundsPoint - Target.TargetTransform.position).magnitude;

                minDistance += distanceToPoint;
                maxDistance += distanceToPoint;

                _orbitMaxRadius = Mathf.Clamp(_orbitMaxRadius + zoomAmount, minDistance, maxDistance);
            } else {
                _orbitMaxRadius += zoomAmount;
            }
        }

        private void Look(Vector3 axes, float dt)
        {
            if(!_enableLook) {
                return;
            }

            _lookRotation.x = MathUtil.WrapAngle(_lookRotation.x + axes.x * _lookSpeedX * dt);
            _lookRotation.y = MathUtil.WrapAngle(_lookRotation.y - axes.y * _lookSpeedY * dt);
        }

        private void FollowTarget(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera.FollowTarget");
            try {
                Quaternion orbitRotation = Quaternion.Euler(_orbitRotation.y, _orbitRotation.x, 0.0f);
                Quaternion lookRotation = Quaternion.Euler(_lookRotation.y, _lookRotation.x, 0.0f);

                Quaternion finalOrbitRotation;
                if(_returnToDefault) {
                    Quaternion targetRotation = Quaternion.Euler(0.0f, Target.TargetTransform.eulerAngles.y, 0.0f);
                    finalOrbitRotation = targetRotation * orbitRotation;
                } else {
                    finalOrbitRotation = orbitRotation;
                }

                transform.rotation = finalOrbitRotation * lookRotation;

                // TODO: this doens't work if we free-look and zoom
                // because we're essentially moving the target position, not the camera position
                _lastTargetPosition = Target.TargetTransform.position;
                _lastTargetPosition = _smooth
                    ? Vector3.SmoothDamp(transform.position, _lastTargetPosition, ref _velocity, _smoothTime)
                    : _lastTargetPosition;

                transform.position = _lastTargetPosition + finalOrbitRotation * new Vector3(0.0f, 0.0f, -_orbitMaxRadius);
            } finally {
                Profiler.EndSample();
            }
        }
    }
}
