using System.Collections;

using pdxpartyparrot.ssjAug2018.Players;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.UI
{
    public sealed class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _infoText;

        [SerializeField]
        private GameObject _aimer;

        [SerializeField]
        private TextMeshProUGUI _timer;

        [SerializeField]
        private TextMeshProUGUI _timeAddedText;

        [SerializeField]
        private float _timeAddedTextDisplaySeconds = 2.0f;

        [SerializeField]
        private TextMeshProUGUI _letterCounter;

        [SerializeField]
        private TextMeshProUGUI _mailboxCounter;

        private Player _owner;

        private Coroutine _hideTimeAddedCoroutine;

#region Unity Lifecycle
        private void Update()
        {
            _aimer.SetActive(_owner.PlayerController.IsAiming);

            _timer.text = $"{GameManager.Instance.RemainingMinutesPart}:{GameManager.Instance.RemainingSecondsPart}";

            _letterCounter.text = $"{_owner.CurrentLetterCount} / {PlayerManager.Instance.PlayerData.MaxLetters}";
            _mailboxCounter.text = "X / Y";
        }
#endregion

        public void Initialize(Player owner)
        {
            _owner = owner;

            _infoText.gameObject.SetActive(false);
            _aimer.SetActive(false);
            _timeAddedText.gameObject.SetActive(false);
        }

        public void ShowTimeAdded(int secondsAdded)
        {
            StopHideTimeAddedCoroutine();

            _timeAddedText.text = $"+{secondsAdded} Seconds";
            _timeAddedText.gameObject.SetActive(true);

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
            _timeAddedText.gameObject.SetActive(false);
        }
    }
}
