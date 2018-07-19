using UnityEngine;
using UnityEngine.EventSystems;

namespace pdxpartyparrot.Core.UI
{
    public sealed class PhysicsRaycastSelector : MonoBehaviour
    {
        [SerializeField]
        private PhysicsRaycaster _physicsRaycaster;
/*
        [SerializeField]
        private GvrPointerPhysicsRaycaster _gvrPhysicsRaycaster;
*/

#region Unity Lifecycle
        private void Awake()
        {
            _physicsRaycaster.enabled = !PartyParrotManager.Instance.EnableVR;
//            _gvrPhysicsRaycaster.enabled = GameManager.Instance.EnableGoogleVR;
        }
#endregion
    }
}
