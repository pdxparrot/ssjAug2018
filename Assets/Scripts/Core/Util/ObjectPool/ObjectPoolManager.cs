using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Util.ObjectPool
{
    public sealed class ObjectPoolManager : SingletonBehavior<ObjectPoolManager>
    {
        private sealed class ObjectPool
        {
            public int Size { get; }

            public string Tag { get; }

            public bool IsNetwork { get; }

            public bool AllowExpand { get; set; } = true;

            public PooledObject Prefab { get; }

            private readonly Queue<PooledObject> _pooledObjects;

            private GameObject _container;

            public ObjectPool(GameObject parent, string tag, PooledObject prefab, int size, bool isNetwork)
            {
                Tag = tag;
                Prefab = prefab;
                Size = size;
                IsNetwork = isNetwork;

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

                if(IsNetwork) {
                    Network.NetworkManager.Instance.SpawnNetworkObject(pooledObject.GetComponent<NetworkBehaviour>());
                }

                return pooledObject;
            }

            public void Recycle(PooledObject pooledObject)
            {
                if(IsNetwork) {
                    Network.NetworkManager.Instance.UnSpawnNetworkObject(pooledObject.GetComponent<NetworkBehaviour>());
                }

                // NOTE: de-activate and then repart to avoid hierarchy rebuild
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.SetParent(_container.transform);

                _pooledObjects.Enqueue(pooledObject);
            }

            private void PopulatePool()
            {
                Assert.IsTrue(!IsNetwork || NetworkServer.active);

                for(int i=0; i<Size; ++i) {
                    PooledObject pooledObject = Instantiate(Prefab);
                    pooledObject.Tag = Tag;
                    Recycle(pooledObject);
                }
            }
        }

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        private GameObject _poolContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _poolContainer = new GameObject("Object Pools");
        }

        protected override void OnDestroy()
        {
            foreach(var kvp in _objectPools) {
                kvp.Value.Destroy();
            }
            _objectPools.Clear();

            Destroy(_poolContainer);
            _poolContainer = null;

            base.OnDestroy();
        }
#endregion

        public void InitializePool(string poolTag, PooledObject prefab, int size, bool allowExpand=true)
        {
            if(null == prefab) {
                Debug.LogError("Attempt to pool non-PooledObject!");
                return;
            }

            NetworkBehaviour networkBehaviour = prefab.GetComponent<NetworkBehaviour>();
            if(null != networkBehaviour) {
                Network.NetworkManager.Instance.RegisterNetworkPrefab(networkBehaviour);

                // network pools are server-only
                if(!NetworkServer.active) {
                    return;
                }
            }

            Debug.Log($"Initializing {(null == networkBehaviour ? "local" : "network")} object pool of size {size} for {poolTag} (allowExpand={allowExpand})");

            ObjectPool objectPool = _objectPools.GetOrDefault(poolTag);
            if(null != objectPool) {
                return;
            }

            objectPool = new ObjectPool(_poolContainer, poolTag, prefab, size, null != networkBehaviour)
            {
                AllowExpand = allowExpand
            };
            _objectPools.Add(poolTag, objectPool);
        }

        public void DestroyPool(string poolTag)
        {
            ObjectPool objectPool = _objectPools.GetOrDefault(poolTag);
            if(null == objectPool) {
                return;
            }

            if(objectPool.IsNetwork) {
                Network.NetworkManager.Instance.UnregisterNetworkPrefab(objectPool.Prefab.GetComponent<NetworkBehaviour>());
            }

            _objectPools.Remove(poolTag);
            objectPool.Destroy();
        }

        [CanBeNull]
        public PooledObject GetPooledObject(string poolTag, Transform parent=null, bool activate=true)
        {
            ObjectPool pool = _objectPools.GetOrDefault(poolTag);
            if(null == pool) {
                Debug.Log($"No pool for tag {poolTag}!");
                return null;
            }
            return pool.GetPooledObject(parent, activate);
        }

        [CanBeNull]
        public T GetPooledObject<T>(string poolTag, Transform parent=null, bool activate=true) where T: Component
        {
            PooledObject po = GetPooledObject(poolTag, parent, activate);
            if(null != po) {
                return po.GetComponent<T>();
            }
            return null;
        }

        public void Recycle(PooledObject pooledObject)
        {
            ObjectPool pool = _objectPools.GetOrDefault(pooledObject.Tag);
            pool?.Recycle(pooledObject);
        }
    }
}
