using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.UI
{
    [RequireComponent(typeof(Canvas))]
    public sealed class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

        public PlayerHUD PlayerHUD => _playerHUD;

        private Canvas _canvas;

#region Unity Lifecycle
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
#endregion

        public void Initialize(Player player)
        {
            _canvas.worldCamera = player.Viewer?.UICamera;
            _playerHUD.Initialize(player);
        }

        public void ToggleHUD()
        {
            _playerHUD.gameObject.SetActive(!_playerHUD.gameObject.activeInHierarchy);
        }
    }
}
