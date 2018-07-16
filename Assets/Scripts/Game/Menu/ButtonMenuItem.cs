using pdxpartyparrot.Core.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonMenuItem : MenuItem
    {
        public abstract void OnClick();
    }
}
