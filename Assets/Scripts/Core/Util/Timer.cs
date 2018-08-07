using System;

namespace pdxpartyparrot.Core.Util
{
    [Serializable]
    public struct Timer
    {
        private float _timerSeconds;

        public float TimerSeconds => _timerSeconds;

        private float _secondsRemaining;

        public float SecondsRemaining => _secondsRemaining;

        private bool _isRunning;

        public bool IsRunning => _isRunning;

        private Action _onTimesUp;

        public void Start(float timerSeconds, Action onTimesUp)
        {
            _onTimesUp = onTimesUp;
            _timerSeconds = timerSeconds;
            _secondsRemaining = _timerSeconds;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Continue()
        {
            _isRunning = true;
        }

        public void AddTime(float seconds)
        {
            _secondsRemaining += seconds;
        }

        public void Update(float dt)
        {
            if(PartyParrotManager.Instance.IsPaused || !_isRunning) {
                return;
            }

            _secondsRemaining -= dt;
            if(_secondsRemaining <= 0.0f) {
                Stop();

                _secondsRemaining = 0.0f;

                _onTimesUp.Invoke();
            }
        }
    }
}
