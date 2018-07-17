using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public sealed class ViewerManager : SingletonBehavior<ViewerManager>
    {
        [SerializeField]
        private float _viewportEpsilon = 0.005f;

        public float ViewportEpsilon => _viewportEpsilon;

#region Viewers
        [SerializeField]
        private Viewer _viewerPrefab;

        private readonly List<Viewer> _viewers = new List<Viewer>();

        private readonly List<Viewer> _assignedViewers = new List<Viewer>();

        private readonly Queue<Viewer> _unassignedViewers = new Queue<Viewer>();
#endregion

        private GameObject _viewerContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _viewerContainer = new GameObject("Viewers");

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            Destroy(_viewerContainer);
            _viewerContainer = null;

            base.OnDestroy();
        }
#endregion

#region Allocate
        public void AllocateViewers(int count)
        {
            int actualCount = count - _viewers.Count;
            if(actualCount <= 0) {
                return;
            }

            Debug.Log($"Allocating {actualCount} viewers...");

            for(int i=0; i<actualCount; ++i) {
                Viewer viewer = Instantiate(_viewerPrefab, _viewerContainer.transform);
                viewer.Initialize(i);
                viewer.gameObject.SetActive(false);

                _viewers.Add(viewer);
                _unassignedViewers.Enqueue(viewer);
            }

            ResizeViewports();
        }

        public void FreeViewers()
        {
            Debug.Log($"Freeing {_viewers.Count} viewers...");

            _assignedViewers.Clear();
            _unassignedViewers.Clear();

            foreach(Viewer viewer in _viewers) {
                Destroy(viewer.gameObject);
            }

            _viewers.Clear();
        }
#endregion

#region Acquire
        [CanBeNull]
        public Viewer AcquireViewer()
        {
            if(_unassignedViewers.Count < 1) {
                return null;
            }

            Viewer viewer = _unassignedViewers.Dequeue();
            viewer.gameObject.SetActive(true);
            _assignedViewers.Add(viewer);

            //Debug.Log($"Acquired viewer {viewer.name}  (assigned: {_assignedViewers.Count}, unassigned: {_unassignedViewers.Count})");
            return viewer;
        }

        public void ReleaseViewer(Viewer viewer)
        {
            if(!_assignedViewers.Contains(viewer)) {
                return;
            }

            //Debug.Log($"Releasing viewer {viewer.name} (assigned: {_assignedViewers.Count}, unassigned: {_unassignedViewers.Count})");

            viewer.Reset();

            viewer.gameObject.SetActive(false);
            _assignedViewers.Remove(viewer);
            _unassignedViewers.Enqueue(viewer);
        }

        public void ResetViewers()
        {
            Debug.Log($"Releasing all ({_assignedViewers.Count}) viewers");

            // we loop through all of the viewers
            // because we can't loop over the assigned viewers
            foreach(Viewer viewer in _viewers) {
                ReleaseViewer(viewer);

                viewer.transform.position = Vector3.zero;
                viewer.transform.rotation = Quaternion.identity;
            }
            ResizeViewports();
        }
#endregion

        public void ResizeViewports()
        {
            if(_assignedViewers.Count > 0) {
                ResizeViewports(_assignedViewers);
            } else if(_unassignedViewers.Count > 0) {
                ResizeViewports(_unassignedViewers);
            }
        }

        private void ResizeViewports(IReadOnlyCollection<Viewer> viewers)
        {
            int gridCols = Mathf.CeilToInt(Mathf.Sqrt(viewers.Count));
            int gridRows = gridCols;

            // remove any extra full colums
            int extraCols = (gridCols * gridRows) - viewers.Count;
            gridCols -= extraCols / gridRows;

            float viewportWidth = (1.0f / gridCols);
            float viewportHeight = (1.0f / gridRows);

            Debug.Log($"Resizing {viewers.Count} viewports, Grid Size: {gridCols}x{gridRows} Viewport Size: {viewportWidth}x{viewportHeight}");

            for(int row=0; row<gridRows; ++row) {
                for(int col=0; col<gridCols; ++col) {
                    int viewerIdx = (row * gridCols) + col;
                    if(viewerIdx >= viewers.Count) {
                        break;
                    }
                    viewers.ElementAt(viewerIdx).SetViewport(col, (gridRows - 1) - row, viewportWidth, viewportHeight);
                }
            }
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ViewerManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Viewers", GUI.skin.box);
                    GUILayout.Label($"Total Viewers: {_viewers.Count}");
                    GUILayout.Label($"Assigned Viewers: {_assignedViewers.Count}");
                    GUILayout.Label($"Unassigned Viewers: {_unassignedViewers.Count}");
                GUILayout.EndVertical();
            };
        }
    }
}
