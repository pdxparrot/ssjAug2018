﻿using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class PauseMenu : Game.Menu.Menu
    {
#region Event Handlers
        public void OnSettings()
        {
            Debug.Log("TODO: settings");
        }

        public void OnExit()
        {
            Application.Quit();
        }
#endregion
    }
}
