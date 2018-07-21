using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    public abstract class UIManager : SingletonBehavior<UIManager>
    {
        [SerializeField]
        private LayerMask _uiLayer;

        public LayerMask UILayer => _uiLayer;

        private GameObject _uiContainer;

        public GameObject UIContainer => _uiContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _uiContainer = new GameObject("UI");
        }

        protected override void OnDestroy()
        {
            Destroy(_uiContainer);
            _uiContainer = null;

            base.OnDestroy();
        }
#endregion
    }
}
