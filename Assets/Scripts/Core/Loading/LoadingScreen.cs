using pdxpartyparrot.Core.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.Core.Loading
{
    [RequireComponent(typeof(Canvas))]
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar _progressBar;

        public ProgressBar Progress => _progressBar;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        public string ProgressText
        {
            get { return _progressText.text; }

            set { _progressText.text = value; }
        }

#region Unity Lifecycle
        private void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 9999;
        }
#endregion
    }
}
