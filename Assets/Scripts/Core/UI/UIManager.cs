using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    public sealed class UIManager : SingletonBehavior<UIManager>
    {
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
