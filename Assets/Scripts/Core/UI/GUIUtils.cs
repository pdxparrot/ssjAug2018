using System.Text.RegularExpressions;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    // TODO: these don't really work, we need to be able to hold the string that is the text
    // but also the float that comes out of it so that typing out 1.0 or -1 or whatever doesn't fuck up

    public static class GUIUtils
    {
        public static int IntField(int currentValue)
        {
            string text = GUILayout.TextField($"{currentValue}");
            text = Regex.Replace(text, @"[^0-9]" ,"");

            int value;
            return int.TryParse(text, out value) ? value : currentValue;
        }

        public static float FloatField(float currentValue)
        {
            string text = GUILayout.TextField($"{currentValue}");
            text = Regex.Replace(text, @"[^0-9\.]" ,"");

            if(text[text.Length - 1] == '.') {
                text += '0';
            }

            float value;
            return float.TryParse(text, out value) ? value : currentValue;
        }
    }
}
