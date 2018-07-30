using System.Collections;

using pdxpartyparrot.ssjAug2018.Players;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.ssjAug2018.UI
{
    public sealed class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _infoText;

        [SerializeField]
        private float _infoTextDisplaySeconds = 2.0f;

        [SerializeField]
        private TextMeshProUGUI _gameOverText;

        [SerializeField]
        private GameObject _aimer;

        [SerializeField]
        private TextMeshProUGUI _timer;

        [SerializeField]
        private GameObject _timeAddedPanel;

        [SerializeField]
        private TextMeshProUGUI _timeAddedText;

        [SerializeField]
        private float _timeAddedTextDisplaySeconds = 2.0f;

        [SerializeField]
        private TextMeshProUGUI _letterCounter;

        [SerializeField]
        private TextMeshProUGUI _mailboxCounter;

        [SerializeField]
        private Image _thrusterFill;

        private Player _owner;

        private Coroutine _hideInfoTextCoroutine;
        private Coroutine _hideTimeAddedCoroutine;

#region Unity Lifecycle
        private void Update()
        {
            _aimer.SetActive(_owner.PlayerController.IsAiming);

            _timer.text = $"{GameManager.Instance.RemainingMinutesPart:00}:{GameManager.Instance.RemainingSecondsPart:00}";

            _letterCounter.text = $"{_owner.CurrentLetterCount} / {PlayerManager.Instance.PlayerData.MaxLetters}";
            _mailboxCounter.text = "X / Y";
            _thrusterFill.fillAmount = 1.0f - _owner.PlayerController.HoverRemainingPercent;
        }
#endregion

        public void Initialize(Player owner)
        {
            _owner = owner;

            _infoText.gameObject.SetActive(false);
            _gameOverText.gameObject.SetActive(false);
            _aimer.SetActive(false);
            _timeAddedPanel.SetActive(false);
        }

        public void ShowInfoText()
        {
            StopHideInfoTextCoroutine();

            _infoText.gameObject.SetActive(true);

            _hideInfoTextCoroutine = StartCoroutine(HideInfoText());
        }

        public void ShowGameOverText()
        {
            _gameOverText.gameObject.SetActive(true);
        }

        public void ShowTimeAdded(int secondsAdded)
        {
            StopHideTimeAddedCoroutine();

            _timeAddedText.text = $"+{secondsAdded} Seconds";
            _timeAddedPanel.SetActive(true);

            _hideTimeAddedCoroutine = StartCoroutine(HideTimeAddedText());
        }

        private void StopHideInfoTextCoroutine()
        {
            if(null == _hideInfoTextCoroutine) {
                return;
            }

            StopCoroutine(_hideInfoTextCoroutine);
            _hideInfoTextCoroutine = null;
        }

        private IEnumerator HideInfoText()
        {
            yield return new WaitForSeconds(_infoTextDisplaySeconds);
            _infoText.gameObject.SetActive(false);
        }

        private void StopHideTimeAddedCoroutine()
        {
            if(null == _hideTimeAddedCoroutine) {
                return;
            }

            StopCoroutine(_hideTimeAddedCoroutine);
            _hideTimeAddedCoroutine = null;
        }

        private IEnumerator HideTimeAddedText()
        {
            yield return new WaitForSeconds(_timeAddedTextDisplaySeconds);
            _timeAddedPanel.SetActive(false);
        }
    }
}
