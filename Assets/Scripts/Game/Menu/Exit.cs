using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class Exit : ButtonMenuItem
    {
#region Event Handlers
        public override void OnClick()
        {
            Application.Quit();
        }
#endregion
    }
}
