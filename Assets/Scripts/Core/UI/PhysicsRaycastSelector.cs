using UnityEngine;
using UnityEngine.EventSystems;

namespace pdxpartyparrot.Core.UI
{
    public sealed class PhysicsRaycastSelector : MonoBehaviour
    {
        [SerializeField]
        private PhysicsRaycaster _physicsRaycaster;

#if USE_GVR
        [SerializeField]
        private GvrPointerPhysicsRaycaster _gvrPhysicsRaycaster;
#endif


#region Unity Lifecycle
        private void Awake()
        {
            _physicsRaycaster.enabled = !PartyParrotManager.Instance.EnableVR;
#if USE_GVR
            _gvrPhysicsRaycaster.enabled = GameManager.Instance.EnableGoogleVR;
#endif
        }
#endregion
    }
}
