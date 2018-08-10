using DG.Tweening;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Core.Tween
{
    public abstract class TweenRunner : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("_runOnAwake")]
        private bool _playOnAwake = true;

        public bool PlayOnAwake { get { return _playOnAwake; } set { _playOnAwake = value; } }

        [SerializeField]
        private bool _resetOnEnable = true;

        public bool ResetOnEnable { get { return _resetOnEnable; } set { _resetOnEnable = value; } }

#region Time Scale
        [SerializeField]
        private float _timeScale = 1.0f;

        [SerializeField]
        private bool _useRandomTimeScale;

        [SerializeField]
        private float _randomTimeScaleMin = 1.0f;

        [SerializeField]
        private float _randomTimeScaleMax = 1.0f;
#endregion

#region Duration
        [SerializeField]
        private float _duration = 1.0f;

        protected float Duration => _duration;

        [SerializeField]
        private bool _useRandomDuration;

        [SerializeField]
        private float _randomDurationMin = 1.0f;

        [SerializeField]
        private float _randomDurationMax = 1.0f;
#endregion

#region Looping
        [SerializeField]
        private int _loops;

        public int Loops { get { return _loops; } set { _loops = value; } }

        public bool IsInfiniteLoop => Loops < 0;

        [SerializeField]
        LoopType _loopType = LoopType.Restart;
#endregion

#region Easing
        [SerializeField]
        private Ease _ease = Ease.Linear;
#endregion

#region Delay
        [SerializeField]
        private float _firstRunDelay = 0.0f;

        [SerializeField]
        private float _delay = 0.0f;
#endregion

#region From / Relative
        [SerializeField]
        private bool _isFrom = false;

        [SerializeField]
        private bool _isRelative = false;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _firstRun = true;

        public bool FirstRun { get { return _firstRun; } set { _firstRun = value; } }

        [CanBeNull]
        private Tweener _tweener;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            InitDuration();
            InitTimeScale();

            if(PlayOnAwake) {
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

        private void InitDuration()
        {
            if(!_useRandomDuration) {
                return;
            }
            _duration = PartyParrotManager.Instance.Random.NextSingle(_randomDurationMin, _randomDurationMax);
        }

        private void InitTimeScale()
        {
            if(!_useRandomTimeScale) {
                return;
            }
            _timeScale = PartyParrotManager.Instance.Random.NextSingle(_randomTimeScaleMin, _randomTimeScaleMax);
        }

        public virtual void Reset()
        {
            Kill();

            InitDuration();
            InitTimeScale();
        }

        public Tweener Play()
        {
            _tweener = CreateTweener();
            if(null == _tweener) {
                return null;
            }
            _tweener.timeScale = _timeScale;

            _tweener.SetEase(_ease)
                .SetDelay(_firstRun ? (_firstRunDelay + _delay) : _delay)
                .SetLoops(_loops, _loopType)
                .SetRecyclable(true);

            if(_isFrom) {
                _tweener.From(_isRelative);
            } else {
                _tweener.SetRelative(_isRelative);
            }

            _firstRun = false;

            return _tweener;
        }

        public void Pause()
        {
            _tweener?.Pause();
        }

        public void Kill()
        {
            _tweener?.Kill();
        }

        protected abstract Tweener CreateTweener();
    }
}
