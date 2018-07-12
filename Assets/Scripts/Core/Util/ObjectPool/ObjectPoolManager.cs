using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.Core.Util.ObjectPool
{
    public sealed class ObjectPoolManager : SingletonBehavior<ObjectPoolManager>
    {
        private sealed class ObjectPool
        {
            public int Size { get; private set; }

            public string Tag { get; private set; }

            public bool AllowExpand { get; set; } = true;

            private readonly PooledObject _prefab;

            private readonly Queue<PooledObject> _pooledObjects;

            private GameObject _container;

            public ObjectPool(ObjectPoolManager parent, string tag, PooledObject prefab, int size)
            {
                Tag = tag;
                _prefab = prefab;
                Size = size;

                _container = new GameObject(Tag);
                _container.transform.SetParent(parent.transform);

                _pooledObjects = new Queue<PooledObject>(Size);
                PopulatePool();
            }

            public void Destroy()
            {
                Object.Destroy(_container);
                _container = null;
            }

            [CanBeNull]
            public PooledObject GetPooledObject(Transform parent=null, bool activate=true)
            {
                if(!_pooledObjects.Any()) {
                    if(!AllowExpand) {
                        return null;
                    }

                    Debug.LogWarning($"Expanding object pool {Tag}!");
                    PopulatePool();
                }

                // NOTE: reparent then activate to avoid hierarchy rebuild
                PooledObject pooledObject = _pooledObjects.Dequeue();
                pooledObject.transform.SetParent(parent);
                pooledObject.gameObject.SetActive(activate);

                return pooledObject;
            }

            public void Recycle(PooledObject pooledObject)
            {
                // NOTE: de-activate and then repart to avoid hierarchy rebuild
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.SetParent(_container.transform);

                _pooledObjects.Enqueue(pooledObject);
            }

            private void PopulatePool()
            {
                for(int i=0; i<Size; ++i) {
                    PooledObject pooledObject = Instantiate(_prefab);
                    pooledObject.Tag = Tag;
                    Recycle(pooledObject);
                }
            }
        }

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

#region Unity Lifecycle
        protected override void OnDestroy()
        {
            foreach(var kvp in _objectPools) {
                kvp.Value.Destroy();
            }
            _objectPools.Clear();

            base.OnDestroy();
        }
#endregion

        public void InitializePool(string poolTag, PooledObject prefab, int size, bool allowExpand=true)
        {
            // TODO: this could do something better I think
            if(_objectPools.ContainsKey(poolTag)) {
                return;
            }

            ObjectPool objectPool = new ObjectPool(this, poolTag, prefab, size)
            {
                AllowExpand = allowExpand
            };
            _objectPools.Add(poolTag, objectPool);
        }

        [CanBeNull]
        public PooledObject GetPooledObject(string poolTag, Transform parent=null, bool activate=true)
        {
            ObjectPool pool = _objectPools.GetOrDefault(poolTag);
            return pool?.GetPooledObject(parent, activate);
        }

        public void Recycle(PooledObject pooledObject)
        {
            ObjectPool pool = _objectPools.GetOrDefault(pooledObject.Tag);
            pool?.Recycle(pooledObject);
        }
    }
}
