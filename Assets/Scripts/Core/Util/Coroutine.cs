using System;
using System.Collections;

using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    // https://www.youtube.com/watch?v=ciDD6Wl-Evk

    public sealed class CoroutineStoppedException : Exception
    {
    }

    public class Coroutine<T>
    {
#region Result
        private T _result;

        private Exception _exception;

        public T Result
        {
            get
            {
                if(null != _exception) {
                    throw _exception;
                }
                return _result;
            }

            private set { _result = value; }
        }
#endregion

        private readonly MonoBehaviour _behavior;

        private Coroutine _coroutine;

        public Coroutine([NotNull] MonoBehaviour behavior)
        {
            _behavior = behavior;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            if(null != _coroutine) {
                StopCoroutine();

                // this is an error
                throw _exception;
            }

            // clear result/exception
            _result = default(T);
            _exception = null;

            // start the coroutine
            _coroutine = _behavior.StartCoroutine(RunCoroutine(coroutine));
        }

        public void StopCoroutine()
        {
            if(null == _coroutine) {
                return;
            }

            // result is now an exception
            _result = default(T);
            _exception = new CoroutineStoppedException();

            // stop the coroutine itself
            _behavior.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator RunCoroutine(IEnumerator coroutine)
        {
            while(true) {
                try {
                    if(!coroutine.MoveNext()) {
                        yield break;
                    }
                } catch(Exception e) {
                    _exception = e;
                    yield break;
                }

                object current = coroutine.Current;
                if(current?.GetType() == typeof(T)) {
                    Result = (T)current;
                } else {
                    yield return current;
                }
            }
        }
    }
}
