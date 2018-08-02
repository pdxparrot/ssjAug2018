using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CreditsData", menuName="pdxpartyparrot/Game/Data/Credits Data")]
    [Serializable]
    public sealed class CreditsData : ScriptableObject
    {
        [Serializable]
        public struct Credits
        {
            public string title;

            public string[] contributor;
        }

        [SerializeField]
        private Credits[] _credits;
    }
}
