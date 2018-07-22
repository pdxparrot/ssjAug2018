using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="ssjAug2018/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : ScriptableObject
    {
#region Controller
        [Header("Controller")]

        [SerializeField]
        private ThirdPersonControllerData _controllerData;

        public ThirdPersonControllerData ControllerData => _controllerData;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        private float _mass = 1.0f;

        public float Mass => _mass;
#endregion
    }
}
