using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="MailboxData", menuName="ssjAug2018/Data/Mailbox Data")]
    [Serializable]
    public sealed class MailboxData : ScriptableObject {

#region Spawn Ranges
        [Header("Spawn Ranges")]

        [SerializeField]
        private float _playerMinRange = 10f;

        public float PlayerMinRange => _playerMinRange;

        [SerializeField]
        private float _playerMaxRange = 500f;
                
        public float PlayerMaxRange => _playerMaxRange;
        
        [SerializeField]
        private float _setMinRange = 3f;
        
        public float SetMinRange => _setMinRange;

        [SerializeField]
        private float _setMaxRange = 30f;

        public float SetMaximumRange => _setMaxRange;
#endregion

        [Space(10)]

#region Set size
        [Header("Mailbox Set Size")]
        [SerializeField]
        private int _setCountMin = 2;

        public int SetCountMin => _setCountMin;

        [SerializeField]
        private int _setCountMax = 5;
        
        public int SetCountMax => _setCountMax;
#endregion

        [Space(10)]

#region Box Properties
        [Header("Mailbox Properties")]
        [SerializeField]
        private int _maxLettersPerBox = 3;

        public int MaxLettersPerBox => _maxLettersPerBox;
#endregion
    }
}