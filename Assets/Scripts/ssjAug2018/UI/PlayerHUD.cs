using System.Collections;

using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.World;

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
        private GameObject _hitMarker;

        [SerializeField]
        private float _hitMarkerDisplaySeconds = 2.0f;

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
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private Image _thrusterFill;

        private Player _owner;

        private Coroutine _hideHitMarkerCoroutine;
        private Coroutine _hideInfoTextCoroutine;
        private Coroutine _hideTimeAddedCoroutine;

#region Unity Lifecycle
        private void Update()
        {
            _timer.text = $"{GameManager.Instance.RemainingMinutesPart:00}:{GameManager.Instance.RemainingSecondsPart:00}";

            _letterCounter.text = $"{_owner.NetworkPlayer.CurrentLetterCount} / {PlayerManager.Instance.PlayerData.MaxLetters}";
            _mailboxCounter.text = $"{MailboxManager.Instance.CompletedMailboxes} / {MailboxManager.Instance.CurrentSetSize}";
            _scoreText.text = $"{_owner.NetworkPlayer.Score:000}";

            _thrusterFill.fillAmount = null == _owner.PlayerController.HoverComponent ? 0.0f : _owner.PlayerController.HoverComponent.RemainingPercent;
        }
#endregion

        public void Initialize(Player owner)
        {
            _owner = owner;

            _aimer.SetActive(false);
            _infoText.gameObject.SetActive(false);
            _gameOverText.gameObject.SetActive(false);
            _aimer.SetActive(false);
            _hitMarker.SetActive(false);
            _timeAddedPanel.SetActive(false);
        }

#region Info Text
        public void ShowInfoText()
        {
            StopHideInfoTextCoroutine();

            _infoText.gameObject.SetActive(true);

            _hideInfoTextCoroutine = StartCoroutine(HideInfoText());
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
#endregion

        public void ShowGameOverText()
        {
            _gameOverText.gameObject.SetActive(true);
        }

        public void ShowAimer(bool show)
        {
            _aimer.SetActive(show);
        }

#region Hit Marker
        public void ShowHitMarker()
        {
            StopHideHitMarkerCoroutine();

            _hitMarker.gameObject.SetActive(true);

            _hideHitMarkerCoroutine = StartCoroutine(HideHitMarker());
        }

        private void StopHideHitMarkerCoroutine()
        {
            if(null == _hideHitMarkerCoroutine) {
                return;
            }

            StopCoroutine(_hideHitMarkerCoroutine);
            _hideHitMarkerCoroutine = null;
        }

        private IEnumerator HideHitMarker()
        {
            yield return new WaitForSeconds(_hitMarkerDisplaySeconds);
            _hitMarker.gameObject.SetActive(false);
        }
#endregion

#region Time Added
        public void ShowTimeAdded(int secondsAdded)
        {
            StopHideTimeAddedCoroutine();

            _timeAddedText.text = $"+{secondsAdded} Seconds";
            _timeAddedPanel.SetActive(true);

            _hideTimeAddedCoroutine = StartCoroutine(HideTimeAddedText());
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
#endregion
    }
}
