using pdxpartyparrot.Game.Actors;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerController : ThirdPersonController
    {
        public void Initialize(Player player)
        {
            base.Initialize(player);

            ControllerData = PlayerManager.Instance.PlayerData.ControllerData;
        }
    }
}
