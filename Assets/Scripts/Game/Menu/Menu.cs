using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    [RequireComponent(typeof(Canvas))]
    public sealed class Menu : MonoBehaviour
    {
#region Unity Lifecycle
        private void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 100;
        }
#endregion
    }
}
