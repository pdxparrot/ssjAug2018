namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class JumpControllerComponent : CharacterActorControllerComponent
    {
        public class JumpAction : CharacterActorControllerAction
        {
            public static JumpAction Default = new JumpAction();
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpAction)) {
                return false;
            }

            if(!Controller.IsGrounded || Controller.IsSliding) {
                return false;
            }

            Controller.Jump(Controller.ControllerData.JumpHeight, Controller.ControllerData.JumpParam);

            return true;
        }
    }
}
