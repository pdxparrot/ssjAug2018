using UnityEditor;
using UnityEngine;

namespace pdxpartyparrot.Core.Editor
{
    public static class EditorUtils
    {
        [MenuItem("PDX Party Parrot/Reset PlayerPrefs")]
        static void ResetPlayerPrefs()
        {
            if(EditorUtility.DisplayDialog("Reset Player Prefs", "Are you sure you wish to reset PlayerPrefs?", "Yes", "No")) {
                PlayerPrefs.DeleteAll();
                EditorUtility.DisplayDialog("Reset Player Prefs", "PlayerPrefs reset!", "Ok");
            }
        }
    }
}
