using DG.Tweening;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.Tween
{
    public sealed class TweenImageFade : TweenRunner
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        [ReadOnly]
        private float _from;

        [SerializeField]
        private float _to = 1.0f;

#region Unity Lifecycle
        protected override void Awake()
        {
            _from = _image.color.a;

            base.Awake();
        }
#endregion

        public override void Reset()
        {
            base.Reset();

            Color color = _image.color;
            color.a = _from;
            _image.color = color;
        }

        protected override Tweener CreateTweener()
        {
            return _image.DOFade(_to, Duration);
        }
    }
}
