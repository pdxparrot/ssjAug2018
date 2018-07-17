using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    [RequireComponent(typeof(Canvas))]
    public class Menu : MonoBehaviour
    {
#region Unity Lifecycle
        protected virtual void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 100;
        }
#endregion
    }
}
