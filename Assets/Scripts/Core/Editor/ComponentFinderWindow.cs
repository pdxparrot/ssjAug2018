using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.UI;

using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.Core.Editor
{
    public class ComponentFinderWindow : Window.EditorWindow
    {
        public enum ComponentType
        {
            None,

            // audio
            AudioSource,

            // physics
            Rigidbody,

            // colliders
            Collider,
            BoxCollider,
            CapsuleCollider,
            SphereCollider,
            MeshCollider,

            // particles
            ParticleSystem,

            // ai
            NavMeshAgent,
            NavMeshObstacle,
        }

        [MenuItem("PDX Party Parrot/Core/Component Finder")]
        static void Init()
        {
            ComponentFinderWindow window = GetWindow<ComponentFinderWindow>();
            window.Show();
        }

        public override string Title => "Component Finder";

        private ComponentType _selectedType = ComponentType.None;

        private readonly List<GameObject> _selectedPrefabs = new List<GameObject>();

        private Vector2 _scrollPosition;

#region Unity Lifecycle
        protected override void OnGUI()
        {
            EditorGUILayout.BeginVertical();
                // TODO: this should be overrideable
                _selectedType = (ComponentType)EditorGUILayout.EnumPopup("Component Type:", _selectedType);

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    foreach(GameObject prefab in _selectedPrefabs) {
                        EditorGUILayout.BeginHorizontal();
                            if(GUIUtils.LayoutButton(prefab.name)) {
                                Selection.activeGameObject = prefab;
                            }
                            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(prefab));
                        EditorGUILayout.EndHorizontal();
                    }
                EditorGUILayout.EndScrollView();

                if(GUIUtils.LayoutButton("Refresh")) {
                    Refresh();
                }
            EditorGUILayout.EndVertical();
        }
#endregion

        protected virtual Type GetSelectedComponentType()
        {
            switch(_selectedType)
            {
            case ComponentType.None:
                return null;
            case ComponentType.AudioSource:
                return typeof(AudioSource);
            case ComponentType.Rigidbody:
                return typeof(Rigidbody);
            case ComponentType.Collider:
                return typeof(Collider);
            case ComponentType.BoxCollider:
                return typeof(BoxCollider);
            case ComponentType.CapsuleCollider:
                return typeof(CapsuleCollider);
            case ComponentType.SphereCollider:
                return typeof(SphereCollider);
            case ComponentType.MeshCollider:
                return typeof(MeshCollider);
            case ComponentType.ParticleSystem:
                return typeof(ParticleSystem);
            case ComponentType.NavMeshAgent:
                return typeof(NavMeshAgent);
            case ComponentType.NavMeshObstacle:
                return typeof(NavMeshObstacle);
            }
            return null;
        }

        private void Refresh()
        {
            _selectedPrefabs.Clear();

            var assetGUIDs = AssetDatabase.FindAssets("t:prefab");
            foreach(string assetGUID in assetGUIDs) {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if(null == prefab) {
                    Debug.LogWarning($"AssetDatabase returned non prefab at {assetPath}");
                    return;
                }

                Type selectedType = GetSelectedComponentType();
                if(null == selectedType || null != prefab.GetComponentInChildren(selectedType)) {
                    _selectedPrefabs.Add(prefab);
                }
            }
        }
    }
}
