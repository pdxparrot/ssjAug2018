using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.UI;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField]
        private Menu _owner;

        protected Menu Owner => _owner;

        [SerializeField]
        private Core.UI.Button _initialSelection;

#region Unity Lifecycle
        private void Update()
        {
            if(null == InputManager.Instance.EventSystem.currentSelectedGameObject || (!InputManager.Instance.EventSystem.currentSelectedGameObject.activeInHierarchy && _initialSelection.gameObject.activeInHierarchy)) {
                _initialSelection.Select();
            }
        }
#endregion

        public void Reset()
        {
            Debug.Log($"TODO: reset menu {name}");

            _initialSelection.Select();
        }
    }
}
