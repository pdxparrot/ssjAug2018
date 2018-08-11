using System;

namespace pdxpartyparrot.Core.Network
{
    public class ClientSceneEventArgs : EventArgs
    {
        public string SceneName { get; }

        public ClientSceneEventArgs(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
