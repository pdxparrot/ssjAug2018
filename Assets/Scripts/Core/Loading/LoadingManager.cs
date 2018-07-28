using System.Collections;

using DG.Tweening;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Scenes;
using pdxpartyparrot.Core.Scripting;
using pdxpartyparrot.Core.Terrain;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;

using UnityEngine;

namespace pdxpartyparrot.Core.Loading
{
    public abstract class LoadingManager<T> : SingletonBehavior<T> where T: LoadingManager<T>
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

#region Manager Prefabs
        [SerializeField]
        private PartyParrotManager _engineManagerPrefab;

        [SerializeField]
        private DebugMenuManager _debugMenuManagerPrefab;

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

        [SerializeField]
        private ObjectPoolManager _objectPoolManagerPrefab;
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
            UpdateLoadingScreen(0.0f, "Creating managers...");
            yield return null;

            CreateManagers();
            yield return null;

            UpdateLoadingScreen(0.5f, "Initializing managers...");
            yield return null;

            InitializeManagers();
            yield return null;

            UpdateLoadingScreen(1.0f, "Loading complete!");

            OnLoad();

            ShowLoadingScreen(false);
        }

        protected virtual void CreateManagers()
        {
            // third party stuff
            DOTween.Init();

            // these managers must come first, in this order
            DebugMenuManager.CreateFromPrefab(_debugMenuManagerPrefab, ManagersContainer);
            PartyParrotManager.CreateFromPrefab(_engineManagerPrefab, ManagersContainer);

            TimeManager.Create(ManagersContainer);
            AudioManager.CreateFromPrefab(_audioManagerPrefab, ManagersContainer);
            ObjectPoolManager.CreateFromPrefab(_objectPoolManagerPrefab, ManagersContainer);
            ViewerManager.CreateFromPrefab(_viewerManagerPrefab, ManagersContainer);
            InputManager.CreateFromPrefab(_inputManagerPrefab, ManagersContainer);
            Instantiate(_networkManagerPrefab, ManagersContainer.transform);
            SceneManager.CreateFromPrefab(_sceneManagerPrefab, ManagersContainer);
            TerrainManager.Create(ManagersContainer);
            ScriptingManager.Create(ManagersContainer);
        }

        protected virtual void InitializeManagers()
        {
        }

        protected virtual void OnLoad()
        {
        }

#region Loading Screen
        public void ShowLoadingScreen(bool show)
        {
            _loadingScreen.gameObject.SetActive(show);
            UpdateLoadingScreen(0.0f, "Loading...");
        }

        public void UpdateLoadingScreen(float percent, string text)
        {
            SetLoadingScreenPercent(percent);
            SetLoadingScreenText(text);
        }

        public void SetLoadingScreenText(string text)
        {
            _loadingScreen.ProgressText = text;
        }

        public void SetLoadingScreenPercent(float percent)
        {
            _loadingScreen.Progress.Percent = Mathf.Clamp01(percent);
        }
#endregion
    }
}
