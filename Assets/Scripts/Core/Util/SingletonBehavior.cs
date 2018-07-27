using UnityEngine;
 
namespace pdxpartyparrot.Core.Util
{
    // http://wiki.unity3d.com/index.php/Singleton

    /// <summary>
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// 
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        private static T _instance;

        public static bool HasInstance => null != _instance && !applicationIsQuitting;

        private static bool applicationIsQuitting;

        public static T Instance
        {
            get
            {
                if(applicationIsQuitting) {
                    Debug.LogError($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }
 
                if(HasInstance) {
                    return _instance;
                }
                _instance = (T)FindObjectOfType(typeof(T));

                if(FindObjectsOfType(typeof(T)).Length > 1) {
                    Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
                    return _instance;
                }
 
                if(!HasInstance) {
                    GameObject singleton = new GameObject();
                    Create(singleton);
                    singleton.name = "(singleton) "+ typeof(T);
 
                    DontDestroyOnLoad(singleton);
 
                    Debug.LogWarning($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
                } else {
                    //Debug.LogWarning($"[Singleton] Using {typeof(T)} instance already created: {_instance.gameObject.name}");
                }

                return _instance;
            }
        }

        public static T Create(GameObject owner)
        {
            if(HasInstance) {
                Debug.LogWarning($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                return _instance;
            }

            _instance = owner.AddComponent<T>();
            return _instance;
        }

        public static T CreateFromPrefab(T prefab, GameObject parent)
        {
            if(HasInstance) {
                Debug.LogWarning($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                return _instance;
            }

            _instance = Instantiate(prefab, parent.transform);
            return _instance;
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
