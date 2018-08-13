using System;

using pdxpartyparrot.ssjAug2018.Actors;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Data
{
    [CreateAssetMenu(fileName="GameData", menuName="pdxpartyparrot/ssjAug2018/Data/Game Data")]
    [Serializable]
    public sealed class GameData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _worldLayer;

        public LayerMask WorldLayer => _worldLayer;

        [SerializeField]
        private ServerSpectator _serverSpectatorPrefab;

        public  ServerSpectator ServerSpectatorPrefab => _serverSpectatorPrefab;

        [SerializeField]
        private float _gameTimeMinutes = 1.0f;

        public float GameTimeSeconds => _gameTimeMinutes * 60.0f;

        [SerializeField]
        private int _scoreGameTimeSeconds = 30;

        public int ScoreGameTimeSeconds => _scoreGameTimeSeconds;

        [SerializeField]
        private float _playerRespawnSeconds = 3.0f;

        public float PlayerRespawnSeconds => _playerRespawnSeconds;

        [SerializeField]
        private bool _playerCollidesMailboxes;

        public bool PlayerCollidesMailboxes => _playerCollidesMailboxes;
    }
}
