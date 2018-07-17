using System.Collections;

using DG.Tweening;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Scenes;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;

using UnityEngine;

namespace pdxpartyparrot.Core.Loading
{
    public abstract class LoadingManager<T> : SingletonBehavior<T> where T: LoadingManager<T>
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

        protected LoadingScreen LoadingScreen => _loadingScreen;

#region Manager Prefabs
        [SerializeField]
        private AudioManager _audioManagerPrefab;

        [SerializeField]
        private ViewerManager _viewerManagerPrefab;

        [SerializeField]
        private InputManager _inputManagerPrefab;

        [SerializeField]
        private NetworkManager _networkManagerPrefab;

        [SerializeField]
        private SceneManager _sceneManagerPrefab;
#endregion

        protected GameObject ManagersContainer { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            ManagersContainer = new GameObject("Managers");
        }

        protected virtual void Start()
        {
            StartCoroutine(Load());
        }
#endregion

        private IEnumerator Load()
        {
            _loadingScreen.Progress.Percent = 0.0f;
            _loadingScreen.ProgressText = "Creating managers...";
            yield return null;

            CreateManagers();
            yield return null;

            _loadingScreen.Progress.Percent = 0.5f;
            _loadingScreen.ProgressText = "Initializing managers...";
            yield return null;

            InitializeManagers();
            yield return null;

            _loadingScreen.Progress.Percent = 1.0f;
            _loadingScreen.ProgressText = "Loading complete!";

            OnLoad();

            Destroy();
        }

        protected virtual void CreateManagers()
        {
            // third party stuff
            DOTween.Init();

            // these managers must come first, in this order
            DebugMenuManager.Create(ManagersContainer);
            PartyParrotManager.Create(ManagersContainer);

            TimeManager.Create(ManagersContainer);
            AudioManager.CreateFromPrefab(_audioManagerPrefab, ManagersContainer);
            ObjectPoolManager.Create(ManagersContainer);
            ViewerManager.CreateFromPrefab(_viewerManagerPrefab, ManagersContainer);
            InputManager.CreateFromPrefab(_inputManagerPrefab, ManagersContainer);
            Instantiate(_networkManagerPrefab, ManagersContainer.transform);
            SceneManager.CreateFromPrefab(_sceneManagerPrefab, ManagersContainer);
            UIManager.Create(ManagersContainer);
        }

        protected virtual void InitializeManagers()
        {
        }

        protected virtual void OnLoad()
        {
        }

        private void Destroy()
        {
            Destroy(_loadingScreen.gameObject);
            _loadingScreen = null;

            Destroy(gameObject);
        }
    }
}
