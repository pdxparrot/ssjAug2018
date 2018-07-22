using System.Collections.Generic;

namespace pdxpartyparrot.Core.Util
{
    public static class ListExtensions
    {
        public static T RemoveFront<T>(this List<T> list)
        {
            T element = list[0];
            list.RemoveAt(0);
            return element;
        }
    }
}
