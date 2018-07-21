using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class CitySpawner : MonoBehaviour
    {
	    [Serializable]
        public struct BlockPrefab
        {
		    public GameObject prefab;
		    public int frequency;
	    }

        [SerializeField]
        private bool _generateOnAwake = true;

	    [SerializeField]
        private List<BlockPrefab> _blockPrefabs;

	    [SerializeField]
        private int _blockDimensions;

	    [SerializeField]
        private Vector2Int _citySize;

        private GameObject _root;

        private readonly System.Random _random = new System.Random();

        // random block spawning
        private float _maxFrequency;

#region Unity Lifecycle
	    private void Start ()
        {
            if(_generateOnAwake) {
                Generate(false);
            }
	    }
#endregion

        public void Generate(bool fromEditorGenerate)
        {
            _maxFrequency = 0.0f;
            for(int i = 0; i < _blockPrefabs.Count; ++i) {
                _maxFrequency += _blockPrefabs[i].frequency;
            }

            Spawn(fromEditorGenerate);
        }

        public void Reset(bool fromEditorReset)
        {
            DestroyRoot(true);
        }

	    private GameObject RandomBlock()
        {
		    var rnd = _random.NextDouble(0.0f, _maxFrequency - 1.0f);
		    int i = 0;

		    for(; i < _blockPrefabs.Count; ++i) {
			    rnd -= _blockPrefabs[i].frequency;
			    if(rnd < 0.0f)
				    break;
		    }

		    return _blockPrefabs[i].prefab;
	    }

        private void Spawn(bool fromEditorGenerate)
        {
            DestroyRoot(fromEditorGenerate);

		    Vector2Int max = new Vector2Int(_citySize.x, _citySize.y);
		    Vector2Int min = new Vector2Int(-max.x, -max.y);

            _root = new GameObject("City");

		    for(int x = min.x; x <= max.x; ++x) {
			    for(int y = min.y; y <= max.y; ++y) {
                    Vector3 pos = new Vector3(x * _blockDimensions, 0,
                                              y * _blockDimensions);

                    Spawn(RandomBlock(), pos, fromEditorGenerate);
			    }
		    }
	    } 

        private GameObject Spawn(GameObject p, Vector3 pos, bool fromEditorGenerate)
        {
#if UNITY_EDITOR
            GameObject block = fromEditorGenerate ? UnityEditor.PrefabUtility.InstantiatePrefab(p) as GameObject : Instantiate(p);
#else
            GameObject block = Instantiate(p);
#endif

            if(null == block) {
                return null;
            }

            block.transform.parent = _root.transform;
            block.transform.position = pos;
            return block;
        }

        private void DestroyRoot(bool fromEditorGenerate)
        {
            DestroyRoot(_root, fromEditorGenerate);
            _root = null;

            GameObject danglingRoot = GameObject.Find("City");
            DestroyRoot(danglingRoot, fromEditorGenerate);
        }

        private void DestroyRoot(GameObject root, bool fromEditorGenerate)
        {
            if(fromEditorGenerate) {
                DestroyImmediate(root);
            } else {
                Destroy(root);
            }
        }
    }
}
