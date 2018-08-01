using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="MailboxData", menuName="ssjAug2018/Data/Mailbox Data")]
    [Serializable]
    public sealed class MailboxData : ScriptableObject
    {
#region Spawn Ranges
        [Header("Spawn Ranges")]

        [Header("Distance from previous set")]
        [SerializeField]
        [Range(0, 250)]
        private float _distanceMinRange = 10.0f;

        public float DistanceMinRange => _distanceMinRange;

        [SerializeField]
        [Range(250, 500)]
        private float _distanceMaxRange = 500f;
                
        public float DistanceMaxRange => _distanceMaxRange;

        [Header("Distance between origin and other mailboxes")]
        [SerializeField]
        [Range(0, 100)]
        private float _setSizeMinRange = 15f;
        
        public float SetMinRange => _setSizeMinRange;

        [SerializeField]
        [Range(100, 500)]
        private float _setSizeMaxRange = 75f;

        public float SetMaxRange => _setSizeMaxRange;
#endregion

        [Space(10)]

#region Set count
        [Header("Mailbox Set Count")]
        [SerializeField]
        [Range(1, 5)]
        private int _setCountMin = 2;

        public int SetCountMin => _setCountMin;

        [SerializeField]
        [Range(5, 10)]
        private int _setCountMax = 5;
        
        public int SetCountMax => _setCountMax;
#endregion

        [Space(10)]

#region Box Properties
        [Header("Mailbox Properties")]
        [SerializeField]
        [Range(1, 10)]
        private int _maxLettersPerBox = 3;

        public int MaxLettersPerBox => _maxLettersPerBox;
#endregion
    }
}