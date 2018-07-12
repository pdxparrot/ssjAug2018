using System.Collections.Generic;

using JetBrains.Annotations;

namespace pdxpartyparrot.Core.Util
{
    public static class DictionaryExtensions
    {
        [CanBeNull]
        public static TV GetOrDefault<TK, TV>(this IReadOnlyDictionary<TK, TV> dict, TK key, TV defaultValue=default(TV))
        {
            TV value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
