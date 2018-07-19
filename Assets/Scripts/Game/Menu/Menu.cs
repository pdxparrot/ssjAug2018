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

        private void Start()
        {
            _initialSelection.Select();
        }
#endregion
    }
}
