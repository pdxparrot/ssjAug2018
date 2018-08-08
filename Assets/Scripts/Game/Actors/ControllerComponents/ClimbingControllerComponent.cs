using System;
using System.Collections;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class ClimbingControllerComponent : CharacterActorControllerComponent
    {
        public class GrabAction : CharacterActorControllerAction
        {
            public static GrabAction Default = new GrabAction();
        }

        public class ReleaseAction : CharacterActorControllerAction
        {
            public static ReleaseAction Default = new ReleaseAction();
        }

        private enum ClimbMode
        {
            None,
            Climbing,
            Hanging
        }

        [SerializeField]
        [ReadOnly]
        private ClimbMode _climbMode = ClimbMode.None;

        public bool IsClimbing => _climbMode != ClimbMode.None;

        [Space(10)]

#region Hands
        [Header("Hands")]

        [SerializeField]
        private Transform _leftHandTransform;

        private RaycastHit? _leftHandHitResult;

        private RaycastHit? _leftHandHangHitResult;

        private bool CanGrabLeft => null != _leftHandHitResult;

        private bool CanHangLeft => null != _leftHandHangHitResult;

        [SerializeField]
        private Transform _rightHandTransform;

        private RaycastHit? _rightHandHitResult;

        private RaycastHit? _rightHandHangHitResult;

        private bool CanGrabRight => null != _rightHandHitResult;

        private bool CanHangRight => null != _rightHandHangHitResult;
#endregion

        [SerializeField]
        private Transform _hangTransform;

        [Space(10)]

#region Head
        [Header("Head")]

        [SerializeField]
        private Transform _headTransform;

        private RaycastHit? _headHitResult;
#endregion

        [Space(10)]

#region Chest
        [Header("Chest")]

        [SerializeField]
        private Transform _chestTransform;

        private RaycastHit? _chestHitResult;
#endregion

        private bool CanClimbUp => IsClimbing && (null == _headHitResult && null != _chestHitResult);

        [Space(10)]

#region Debug
        [Header("Debug")]

        [SerializeField]
        [Tooltip("Debug break when grabbing fails")]
        private bool _breakOnFall;

        private DebugMenuNode _debugMenuNode;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Debug.Assert(Math.Abs(_leftHandTransform.position.y - _rightHandTransform.position.y) < float.Epsilon, "Character hands are at different heights!");
            Debug.Assert(_headTransform.position.y > _leftHandTransform.position.y, "Character head should be above player hands!");
            Debug.Assert(_chestTransform.position.y < _leftHandTransform.position.y, "Character chest should be below player hands!");

            StartCoroutine(RaycastRoutine());

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            base.OnDestroy();
        }

        private void OnDrawGizmos()
        {
            // left hand
            Gizmos.color = null != _leftHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + transform.forward * Controller.ControllerData.ArmRayLength);

            Gizmos.color = null != _leftHandHangHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (transform.up) * Controller.ControllerData.HangRayLength);

            if(IsClimbing) {
                //if(!CanGrabLeft && CanGrabRight) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 2.0f);
                //}

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 0.5f);
            }

            // right hand
            Gizmos.color = null != _rightHandHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + transform.forward * Controller.ControllerData.ArmRayLength);

            Gizmos.color = null != _rightHandHangHitResult ? Color.red : Color.yellow;
            Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (transform.up) * Controller.ControllerData.HangRayLength);

            if(IsClimbing) {
                //if(CanGrabLeft && !CanGrabRight) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 2.0f);
                //}

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 0.5f);
            }

            if(IsClimbing) {
                // head
                Gizmos.color = null != _headHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_headTransform.position, _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength);

                if(!CanGrabLeft && !CanGrabRight && CanClimbUp) {
                    Gizmos.color = Color.white;
                    Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength;
                    Vector3 end = start + Controller.Owner.Height * -Vector3.up;
                    Gizmos.DrawLine(start, end);
                }

                // chest
                Gizmos.color = null != _chestHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_chestTransform.position, _chestTransform.position + transform.forward * Controller.ControllerData.ChestRayLength);
            }

            // feet
/*
            if(!IsGrabbing && IsGrounded) {
                Gizmos.color = null != _footHitResults[0] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(0.0f, transform.up) * Quaternion.AngleAxis(Controller.ControllerData.FootRayAngle, transform.right) * transform.forward) * Controller.ControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[1] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * Quaternion.AngleAxis(Controller.ControllerData.FootRayAngle, transform.right) * transform.forward) * Controller.ControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[2] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(180.0f, transform.up) * Quaternion.AngleAxis(Controller.ControllerData.FootRayAngle, transform.right) * transform.forward) * Controller.ControllerData.FootRayLength);
                Gizmos.color = null != _footHitResults[3] ? Color.red : Color.yellow;
                Gizmos.DrawLine(_footTransform.position, _footTransform.position + (Quaternion.AngleAxis(270.0f, transform.up) * Quaternion.AngleAxis(Controller.ControllerData.FootRayAngle, transform.right) * transform.forward) * Controller.ControllerData.FootRayLength);
            }
*/
        }
#endregion

        public override bool OnAnimationMove(Vector3 axes, float dt)
        {
            switch(_climbMode)
            {
            case ClimbMode.None:
                return false;
            case ClimbMode.Climbing:
                return true;
            case ClimbMode.Hanging:
                Controller.DefaultAnimationMove(axes, dt);
                return true;
            }
            return false;
        }

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsClimbing) {
                return false;
            }

            switch(_climbMode)
            {
            case ClimbMode.Climbing:
                Vector3 velocity = Controller.Rigidbody.rotation * (axes * Controller.ControllerData.ClimbSpeed);
                if(Controller.DidGroundCheckCollide && velocity.y < 0.0f) {
                    velocity.y = 0.0f;
                }
                Controller.Rigidbody.MovePosition(Controller.Rigidbody.position + velocity * dt);
                break;
            case ClimbMode.Hanging:
                Controller.DefaultPhysicsMove(axes, Controller.ControllerData.HangSpeed, dt);
                break;
            }

            return true;
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(action is GrabAction) {
                if(IsClimbing) {
                    return true;
                }

                if(CanGrabLeft && CanGrabRight) {
                    StartClimbing();

                    if(null != _leftHandHitResult) {
                        AttachToSurface(_leftHandHitResult.Value, false);
                    } else if(null != _rightHandHitResult) {
                        AttachToSurface(_rightHandHitResult.Value, false);
                    }
                } else if(CanHangLeft && CanHangRight) {
                    StartHanging();

                    if(null != _leftHandHangHitResult) {
                        AttachToSurface(_leftHandHangHitResult.Value, true);
                    } else if(null != _rightHandHangHitResult) {
                        AttachToSurface(_rightHandHangHitResult.Value, true);
                    }
                } else {
                    return false;
                }

                return true;
            }

            if(action is ReleaseAction) {
                if(IsClimbing) {
                    StopClimbing();
                } else {
                    CheckDropDown();
                }

                return true;
            }

            return false;
        }

        private void StartClimbing()
        {
            _climbMode = ClimbMode.Climbing;
            Controller.Rigidbody.isKinematic = true;

            Controller.Owner.Animator.SetBool(Controller.ControllerData.ClimbingParam, true);
            Controller.Owner.Animator.SetBool(Controller.ControllerData.HangingParam, false);
        }

        private void StartHanging()
        {
            _climbMode = ClimbMode.Hanging;
            Controller.Rigidbody.isKinematic = true;

            Controller.Owner.Animator.SetBool(Controller.ControllerData.ClimbingParam, false);
            Controller.Owner.Animator.SetBool(Controller.ControllerData.HangingParam, true);
        }


        public void StopClimbing()
        {
            _climbMode = ClimbMode.None;
            Controller.Rigidbody.isKinematic = false;

            Controller.Owner.Animator.SetBool(Controller.ControllerData.ClimbingParam, false);
            Controller.Owner.Animator.SetBool(Controller.ControllerData.HangingParam, false);

            // fix our orientation, just in case
            Vector3 rotation = transform.eulerAngles;
            rotation.x = rotation.z = 0.0f;
            transform.eulerAngles = rotation;
        }

        private IEnumerator RaycastRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(Controller.RaycastRoutineRate);
            while(true) {
                UpdateRaycasts();

                HandleRaycasts();

                yield return wait;
            }
        }

        private void UpdateRaycasts()
        {
            if(Controller.IsAnimating) {
                return;
            }

            Profiler.BeginSample("PlayerController.UpdateRaycasts");
            try {
                UpdateHandRaycasts();
                UpdateHeadRaycasts();
                UpdateChestRaycasts();
            } finally {
                Profiler.EndSample();
            }
        }

#region Hand Raycasts
        private void UpdateHandRaycasts()
        {
            UpdateLeftHandRaycasts();
            UpdateRightHandRaycasts();
        }

        private void UpdateLeftHandRaycasts()
        {
            _leftHandHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_leftHandTransform.position, transform.forward, out hit, Controller.ControllerData.ArmRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHitResult = hit;
                }
            }

            _leftHandHangHitResult = null;

            if(Physics.Raycast(_leftHandTransform.position, transform.up, out hit, Controller.ControllerData.HangRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHangHitResult = hit;
                }
            }
        }

        private void UpdateRightHandRaycasts()
        {
            _rightHandHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_rightHandTransform.position, transform.forward, out hit, Controller.ControllerData.ArmRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHitResult = hit;
                }
            }

            _rightHandHangHitResult = null;

            if(Physics.Raycast(_rightHandTransform.position, transform.up, out hit, Controller.ControllerData.HangRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHangHitResult = hit;
                }
            }
        }
#endregion

#region Head Raycasts
        private void UpdateHeadRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _headHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_headTransform.position, Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward, out hit, Controller.ControllerData.HeadRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _headHitResult = hit;
                }
            }
        }
#endregion

#region Chest Raycasts
        private void UpdateChestRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _chestHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_chestTransform.position, transform.forward, out hit, Controller.ControllerData.ChestRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _chestHitResult = hit;
                }
            }
        }
#endregion

/*
#region Foot Raycasts
        private void UpdateFootRaycasts()
        {
            if(IsClimbing || !IsGrounded) {
                return;
            }

            UpdateFootRaycast(0, 0.0f);
            UpdateFootRaycast(1, 90.0f);
            UpdateFootRaycast(2, 180.0f);
            UpdateFootRaycast(3, 270.0f);
        }

        private void UpdateFootRaycast(int idx, float angle)
        {
            _footHitResults[idx] = null;

            RaycastHit hit;
            if(Physics.Raycast(_footTransform.position, Quaternion.AngleAxis(angle, transform.up) * Quaternion.AngleAxis(Controller.ControllerData.FootRayAngle, transform.right) * transform.forward, out hit, Controller.ControllerData.FootRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                _footHitResults[idx] = hit;
            }
        }
#endregion
*/
        private void HandleRaycasts()
        {
            Profiler.BeginSample("PlayerController.HandleRaycasts");
            try {
                if(IsClimbing) {
                    HandleClimbingRaycasts();
                }
            } finally {
                Profiler.EndSample();
            }
        }

        private void HandleClimbingRaycasts()
        {
            // check for wrapping / climbing up
            if(ClimbMode.Hanging == _climbMode) {
                if(!CanHangLeft && !CanHangRight) {
                    Debug.LogWarning("Unexpectedly fell off!");
                    StopClimbing();

                    if(_breakOnFall) {
                        Debug.Break();
                    }
                } if(CanHangLeft != CanHangRight) {
                    // TODO: check to see if we can climb up
                }
            } else {
                if(!CanGrabLeft && CanGrabRight) {
                    CheckWrapLeft();
                } else if(CanGrabLeft && !CanGrabRight) {
                    CheckWrapRight();
                } else if(!CanGrabLeft && !CanGrabRight && ClimbMode.Hanging != _climbMode) {
                    if(CanClimbUp) {
                        CheckClimbUp();
                    } else {
                        Debug.LogWarning("Unexpectedly fell off!");
                        StopClimbing();

                        if(_breakOnFall) {
                            Debug.Break();
                        }
                    }
                }
            }

            // check for hanging and non-wrap rotations
            if(ClimbMode.Hanging == _climbMode) {
                // TODO: check shit here
            } else if(!CheckHang()) {
                if(!CheckRotateLeft()) {
                    CheckRotateRight();
                }
            }
        }

#region Auto-Rotate/Climb
// TODO: encapsulate the common code from these

        private bool CheckWrapLeft()
        {
            if(null == _rightHandHitResult || Controller.Driver.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward, out hit, Controller.ControllerData.ArmRayLength * 2.0f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckRotateLeft()
        {
            if(null == _rightHandHitResult || Controller.Driver.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward, out hit, Controller.ControllerData.ArmRayLength * 0.5f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckWrapRight()
        {
            if(null == _leftHandHitResult || Controller.Driver.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(-Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward, out hit, Controller.ControllerData.ArmRayLength * 2.0f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckRotateRight()
        {
            if(null == _leftHandHitResult || Controller.Driver.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            RaycastHit hit;
            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(90.0f, transform.up) * transform.forward, out hit, Controller.ControllerData.ArmRayLength * 0.5f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckClimbUp()
        {
            if(Controller.Driver.LastMoveAxes.y <= 0.0f) {
                return false;
            }

            // cast a ray from the end of our rotated head check straight down to see if we can stand here
            Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength;
            float length = Controller.Owner.Height;

            RaycastHit hit;
            if(!Physics.Raycast(start, -Vector3.up, out hit, length, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            StopClimbing();

            Vector3 offset = Controller.Owner.Radius * 2.0f * transform.forward;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition(hit, offset), Controller.Rigidbody.rotation, Controller.ControllerData.ClimbUpTimeSeconds);

            return true;
        }

        private bool CheckHang()
        {
            if((!CanHangLeft && !CanHangRight) || Controller.Driver.LastMoveAxes.y <= 0.0f) {
                return false;
            }

            StartHanging();

            Vector3 offset = -_hangTransform.localPosition;
            Controller.StartAnimation(Controller.Rigidbody.position + GetSurfaceAttachmentPosition((_leftHandHangHitResult ?? _rightHandHangHitResult).Value, offset), Controller.Rigidbody.rotation, Controller.ControllerData.HangTimeSeconds);

            return true;
        }

        private bool CheckDropDown()
        {
            if(Controller.Driver.LastMoveAxes.y >= 0.0f) {
                return false;
            }

Debug.Log("TODO: check drop down");
            return false;
        }
#endregion

        private Vector3 GetSurfaceAttachmentPosition(RaycastHit hit, Vector3 offset)
        {
            // keep a set distance away from the surface
            Vector3 targetPoint = hit.point + (hit.normal * Controller.ControllerData.AttachDistance);
            Vector3 a = targetPoint - Controller.Rigidbody.position;
            Vector3 p = Vector3.Project(a, hit.normal);

            return p + offset;
        }

        private void AttachToSurface(RaycastHit hit, bool isHang)
        {
            Debug.Log($"Attach to surface {hit.transform.name}");

            // TODO: we can probably infer this using the dot product
            if(isHang) {
                Controller.Rigidbody.position += GetSurfaceAttachmentPosition(hit, -_hangTransform.localPosition);
            } else {
                transform.forward = -hit.normal;
                Controller.Rigidbody.position += GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal);
            }
        }

        private void DropDown()
        {
Debug.Log("TODO: drop down");
        }

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Player {Controller.Owner.Name} Climbing Component");
            _debugMenuNode.RenderContentsAction = () => {
                _breakOnFall = GUILayout.Toggle(_breakOnFall, "Break on fall");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
