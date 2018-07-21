using System;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="ssjAug2018/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : ScriptableObject
    {
#region Physics
        [SerializeField]
        private float _moveSpeed = 1.0f;

        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        private float _jumpHeight = 1.0f;

        public float JumpHeight => _jumpHeight;
#endregion
    }
}
