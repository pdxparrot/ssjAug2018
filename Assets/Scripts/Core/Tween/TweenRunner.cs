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

#region Duration
        [SerializeField]
        private float _duration = 1.0f;

        protected float Duration => _duration;
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

        [SerializeField]
        [ReadOnly]
        private bool _firstRun = true;

        public bool FirstRun { get { return _firstRun; } set { _firstRun = value; } }

        [CanBeNull]
        private Tweener _tweener;

#region Unity Lifecycle
        protected virtual void Awake()
        {
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

        public virtual void Reset()
        {
            Kill();
        }

        public Tweener Play()
        {
            _tweener = CreateTweener()
                .SetEase(_ease)
                .SetDelay(_firstRun ? (_firstRunDelay + _delay) : _delay)
                .SetLoops(_loops, _loopType)
                .SetRecyclable(true);

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
