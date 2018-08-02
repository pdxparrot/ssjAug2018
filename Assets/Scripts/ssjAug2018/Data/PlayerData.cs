using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="pdxpartyparrot/ssjAug2018/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : ScriptableObject
    {
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

#region Inventory
        [SerializeField]
        private int _maxLetters = 1;

        public int MaxLetters => _maxLetters;

        [SerializeField]
        private float _reloadTimeSeconds = 5.0f;

        public float ReloadTimeSeconds => _reloadTimeSeconds;

        public int ReloadTimeMs => (int)(_reloadTimeSeconds * 1000.0f);
#endregion
    }
}
