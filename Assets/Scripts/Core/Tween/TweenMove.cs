using DG.Tweening;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Tween
{
    public sealed class TweenMove : TweenRunner
    {
        [SerializeField]
        [ReadOnly]
        private Vector3 _from;

        [SerializeField]
        private Vector3 _to;

        [SerializeField]
        private bool _snapping;

        [SerializeField]
        private bool _useLocalPosition = true;

#region Unity Lifecycle
        protected override void Awake()
        {
            _from = _useLocalPosition ? transform.localPosition : transform.position;

            base.Awake();
        }
#endregion

        public override void Reset()
        {
            base.Reset();

            if(_useLocalPosition) {
                transform.localPosition = _from;
            } else {
                transform.position = _from;
            }
        }

        protected override Tweener CreateTweener()
        {
            return _useLocalPosition
                ? transform.DOLocalMove(_to, Duration, _snapping)
                : transform.DOMove(_to, Duration, _snapping);
        }
    }
}
