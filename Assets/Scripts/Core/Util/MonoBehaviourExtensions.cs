using System.Collections;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public static class MonoBehaviourExtensions
    {
        // https://www.youtube.com/watch?v=ciDD6Wl-Evk
        public static Coroutine<T> StartCoroutine<T>(this MonoBehaviour behavior, IEnumerator coroutine)
        {
            Coroutine<T> routine = new Coroutine<T>(behavior);
            routine.StartCoroutine(coroutine);
            return routine;
        }
    }
}
