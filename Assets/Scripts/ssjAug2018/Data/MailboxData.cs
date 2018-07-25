using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="MailboxData", menuName="ssjAug2018/Data/Mailbox Data")]
    [Serializable]
    public sealed class MailboxData : ScriptableObject {

#region Spawn Ranges
        [Header("Spawn Ranges")]

        [Header("Distance from previous set")]
        [SerializeField]
        private float _distanceMinRange = 10f;

        public float DistanceMinRange => _distanceMinRange;

        [SerializeField]
        private float _distanceMaxRange = 500f;
                
        public float DistanceMaxRange => _distanceMaxRange;
        
        [Header("Distance between origin and other mailboxes")]
        [SerializeField]
        private float _setSizeMinRange = 3f;
        
        public float SetMinRange => _setSizeMinRange;

        [SerializeField]
        private float _setSizeMaxRange = 30f;

        public float SetMaxRange => _setSizeMaxRange;
#endregion

        [Space(10)]

#region Set count
        [Header("Mailbox Set Count")]
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


#region Randomization
        [Header("Randomization")]
        [SerializeField]
        private int _randomizationSeed = 0;
        
        public int RandomizationSeed => _randomizationSeed;
#endregion

    }
}