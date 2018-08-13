using UnityEngine;

namespace pdxpartyparrot.Core.Editor.Window
{
    public abstract class EditorWindow : UnityEditor.EditorWindow
    {
/*
        [MenuItem("PDX Party Parrot/Window")]
        static void Init()
        {
            ComponentFinderWindow window = GetWindow<CustomWindowType>();
            window.Show();
        }
*/

        public abstract string Title { get; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            titleContent = new GUIContent(Title);
        }

        protected virtual void OnGUI()
        {
        }
#endregion
    }
}
