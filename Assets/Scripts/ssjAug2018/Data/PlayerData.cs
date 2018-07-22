using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="ssjAug2018/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : ScriptableObject
    {
        [SerializeField]
        private ThirdPersonControllerData _controllerData;

        public ThirdPersonControllerData ControllerData => _controllerData;
    }
}
