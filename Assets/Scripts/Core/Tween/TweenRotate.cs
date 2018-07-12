using DG.Tweening;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Tween
{
    public sealed class TweenRotate : TweenRunner
    {
        [SerializeField]
        [ReadOnly]
        private Vector3 _from;

        [SerializeField]
        private Vector3 _to;

        [SerializeField]
        private RotateMode _rotateMode = RotateMode.Fast;

        [SerializeField]
        private bool _useLocalRotation = true;

#region Unity Lifecycle
        protected override void Awake()
        {
            _from = _useLocalRotation ? transform.localEulerAngles : transform.eulerAngles;

            base.Awake();
        }
#endregion

        public override void Reset()
        {
            base.Reset();

            if(_useLocalRotation) {
                transform.localEulerAngles = _from;
            } else {
                transform.eulerAngles = _from;
            }
        }

        protected override Tweener CreateTweener()
        {
            return _useLocalRotation
                ? transform.DOLocalRotate(_to, Duration, _rotateMode)
                : transform.DORotate(_to, Duration, _rotateMode);
        }
    }
}
