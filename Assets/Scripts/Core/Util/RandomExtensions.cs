using System;
using System.Collections.Generic;
using System.Linq;

namespace pdxpartyparrot.Core.Util
{
    public static class RandomExtensions
    {
#region Random Collection Entries
        public static T GetRandomEntry<T>(this Random random, IReadOnlyCollection<T> collection)
        {
            if(collection.Count < 1) {
                return default(T);
            }

            int idx = random.Next(collection.Count);
            return collection.ElementAt(idx);
        }

        public static T RemoveRandomEntry<T>(this Random random, IList<T> collection)
        {
            if(collection.Count < 1) {
                return default(T);
            }

            int idx = random.Next(collection.Count);
            T v = collection.ElementAt(idx);
            collection.RemoveAt(idx);
            return v;
        }
#endregion

        public static float NextSign(this Random random)
        {
            return random.Next(0, 1) == 0 ? -1 : 1;
        }

        public static float NextSingle(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return minValue + random.NextDouble() * (maxValue - minValue);
        }

        public static float NextSingle(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble(minValue, maxValue);
        }
    }
}
