using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="ThirdPersonControllerData", menuName="ssjAug2018/Data/ThirdPersonController Data")]
    [Serializable]
    public sealed class ThirdPersonControllerData : ScriptableObject
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
