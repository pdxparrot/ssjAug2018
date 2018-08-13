using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ssjAug2018.Camera;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="pdxpartyparrot/ssjAug2018/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _playerLayer;

        public LayerMask PlayerLayer => _playerLayer;

        [SerializeField]
        private LayerMask _viewerLayer;

        public LayerMask ViewerLayer => _viewerLayer;

        [Space(10)]

#region Viewers
        [SerializeField]
        [FormerlySerializedAs("_viewerPrefab")]
        private Camera.Viewer _playerViewerPrefab;

        public Camera.Viewer PlayerViewerPrefab => _playerViewerPrefab;

        [SerializeField]
        private ServerSpectatorViewer _serverSpectatorViewer;

        public ServerSpectatorViewer ServerSpectatorViewer => _serverSpectatorViewer;
#endregion

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _stunnedParam = "Stunned";

        public string StunnedParam => _stunnedParam;

        [SerializeField]
        private string _deadParam = "Dead";

        public string DeadParam => _deadParam;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        private float _mass = 1.0f;

        public float Mass => _mass;

        [SerializeField]
        private float _drag = 0.0f;

        public float Drag => _drag;

        [SerializeField]
        private float _angularDrag = 0.0f;

        public float AngularDrag => _angularDrag;
#endregion

        [Space(10)]

#region Controls
        [Header("Controls")]

        [SerializeField]
        private float _movementLerpSpeed = 1.0f;

        public float MovementLerpSpeed => _movementLerpSpeed;

        [SerializeField]
        private float _lookLerpSpeed = 1.0f;

        public float LookLerpSpeed => _lookLerpSpeed;
#endregion

        [Space(10)]

#region Inventory
        [Header("Inventory")]

        [SerializeField]
        private int _maxLetters = 1;

        public int MaxLetters => _maxLetters;

        [SerializeField]
        private float _reloadTimeSeconds = 5.0f;

        public float ReloadTimeSeconds => _reloadTimeSeconds;
#endregion
    }
}
