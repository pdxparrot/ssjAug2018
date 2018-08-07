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
        private LayerMask _mailLayer;

        public LayerMask MailLayer => _mailLayer;

        [SerializeField]
        private float _mailDespawnSeconds = 5.0f;

        public float MailDespawnSeconds => _mailDespawnSeconds;

        [SerializeField]
        private int _mailScoreAmount = 1;

        public int MailScoreAmount => _mailScoreAmount;
#endregion
    }
}
