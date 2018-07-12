using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
    public sealed class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image _background;

        [SerializeField]
        private Image _foreground;

        [SerializeField]
        private float _percent;

        public float Percent
        {
            get { return _percent; }

            set { _percent = Mathf.Clamp01(value); }
        }

#region Unity Lifecycle
        private void Update()
        {
            _foreground.fillAmount = Percent;
        }
#endregion
    }
}
