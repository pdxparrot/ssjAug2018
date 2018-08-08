using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018
{
    public sealed class HighScoreManager : SingletonBehavior<HighScoreManager>
    {
        public struct HighScore : IComparable<HighScore>
        {
            public string playerName;

            public int score;

            public int CompareTo(HighScore other)
            {
                return score.CompareTo(other.score);
            }
        }

        private readonly SortedSet<HighScore> _highScores = new SortedSet<HighScore>();

        public IReadOnlyCollection<HighScore> HighScores => _highScores;

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }
#endregion

        public void AddHighScore(string playerName, int score)
        {
            _highScores.Add(new HighScore
            {
                playerName = playerName,
                score = score
            });
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.HighScoreManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("High Scores", GUI.skin.box);
                    foreach(HighScore highScore in HighScores) {
                        GUILayout.Label($"{highScore.playerName}: {highScore.score}");
                    }
                GUILayout.EndVertical();
            };
        }
    }
}
