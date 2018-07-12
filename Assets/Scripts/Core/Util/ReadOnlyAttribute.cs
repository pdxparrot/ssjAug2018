 using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    // http://answers.unity3d.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html

    public sealed class ReadOnlyAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public sealed class ReadOnlyDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);
        }
 
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
                UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
