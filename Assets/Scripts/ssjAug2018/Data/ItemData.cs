using System;

using pdxpartyparrot.ssjAug2018.Items;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="ItemData", menuName="pdxpartyparrot/ssjAug2018/Data/Item Data")]
    [Serializable]
    public sealed class ItemData : ScriptableObject
    {
        [SerializeField]
        private bool _thrownItemsUseGravity;

        public bool ThrownItemsUseGravity => _thrownItemsUseGravity;

#region Items
        [Header("Items")]

        [SerializeField]
        private Mail _mailPrefab;

        public Mail MailPrefab => _mailPrefab;

        [SerializeField]
        private float _mailDespawnSeconds = 5.0f;

        public int MailDespawnMs => (int)(_mailDespawnSeconds * 1000.0f);

        [SerializeField]
        private int _mailScoreAmount = 1;

        public int MailScoreAmount => _mailScoreAmount;
#endregion
    }
}
