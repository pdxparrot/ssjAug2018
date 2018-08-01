using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    [RequireComponent(typeof(Canvas))]
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button _initialSelection;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 100;
        }

        private void Update()
        {
            if(null == InputManager.Instance.EventSystem.currentSelectedGameObject) {
                _initialSelection.Select();
            }
        }

        private void Start()
        {
            Reset();
        }
#endregion

        public void Reset()
        {
            Debug.Log($"TODO: reset menu {name}");

            _initialSelection.Select();
        }
    }
}
