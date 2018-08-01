using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class SceneTester : BaseGame
    {
        [SerializeField]
        private string[] _testScenes;

        public string[] TestScenes => _testScenes;

        public void SetScene(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
