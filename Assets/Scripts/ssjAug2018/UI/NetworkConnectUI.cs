using pdxpartyparrot.ssjAug2018.GameState;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.UI
{
    [RequireComponent(typeof(Canvas))]
    public sealed class NetworkConnectUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _connectionStatusText;

        private NetworkConnect _owner;

#region Unity Lifecycle
        private void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 500;
        }
#endregion

        public void Initialize(NetworkConnect owner)
        {
            _owner = owner;
        }

        public void SetStatus(string status)
        {
            _connectionStatusText.text = status;
        }

#region Event Handlers
        public void OnCancel()
        {
            _owner.Cancel();
        }
#endregion
    }
}
