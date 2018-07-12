using DG.Tweening;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Tween
{
    public class TweenSequence : MonoBehaviour
    {
        [SerializeField]
        private bool _playOnAwake = true;

        [SerializeField]
        private bool _resetOnEnable = true;

#region Looping
        [SerializeField]
        private int _loops = 0;

        [SerializeField]
        LoopType _loopType = LoopType.Restart;
#endregion

#region Delay
        [SerializeField]
        private float _firstRunDelay = 0.0f;

        [SerializeField]
        private float _delay = 0.0f;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _firstRun = true;

        [SerializeField]
        private TweenRunner[] _tweens;

        [CanBeNull]
        private Sequence _sequence;

#region Unity Lifecycle
        private void Awake()
        {
            foreach(TweenRunner runner in _tweens) {
                // cleanup the runner start states so they don't act outside our control
                // TODO: this doesn't work :( bleh...
                runner.PlayOnAwake = false;
                runner.ResetOnEnable = false;
                runner.FirstRun = false;

                if(runner.IsInfiniteLoop) {
                    runner.Loops = 0;
                }
            }

            if(_playOnAwake) {
                Play();
            }
        }

        protected virtual void OnEnable()
        {
            if(_resetOnEnable) {
                Reset();
                Play();
            }
        }
#endregion

        private void Reset()
        {
            Kill();

            foreach(TweenRunner runner in _tweens) {
                runner.Reset();
            }
        }

        public void Play()
        {
            _sequence = DOTween.Sequence()
                .SetDelay(_firstRun ? (_firstRunDelay + _delay) : _delay)
                .SetLoops(_loops, _loopType);

            foreach(TweenRunner runner in _tweens) {
                _sequence.Append(runner.Play());
            }

            _sequence.Play();

            _firstRun = false;
        }

        public void Pause()
        {
            _sequence?.Pause();
        }

        public void Kill()
        {
            _sequence?.Kill();
        }
    }
}
