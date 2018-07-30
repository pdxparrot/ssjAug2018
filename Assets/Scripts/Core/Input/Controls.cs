// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Controls.inputactions'

namespace pdxpartyparrot.Core.Input
{
    [System.Serializable]
    public class Controls : UnityEngine.Experimental.Input.InputActionWrapper
    {
        private bool m_Initialized;
        private void Initialize()
        {
            // game
            m_game = asset.GetActionMap("game");
            m_game_move = m_game.GetAction("move");
            m_game_look = m_game.GetAction("look");
            m_game_jump = m_game.GetAction("jump");
            m_game_pause = m_game.GetAction("pause");
            m_game_grab = m_game.GetAction("grab");
            m_game_drop = m_game.GetAction("drop");
            m_game_throwmail = m_game.GetAction("throw mail");
            m_game_hover = m_game.GetAction("hover");
            m_game_aim = m_game.GetAction("aim");
            m_game_throwsnowball = m_game.GetAction("throw snowball");
            m_Initialized = true;
        }
        // game
        private UnityEngine.Experimental.Input.InputActionMap m_game;
        private UnityEngine.Experimental.Input.InputAction m_game_move;
        private UnityEngine.Experimental.Input.InputAction m_game_look;
        private UnityEngine.Experimental.Input.InputAction m_game_jump;
        private UnityEngine.Experimental.Input.InputAction m_game_pause;
        private UnityEngine.Experimental.Input.InputAction m_game_grab;
        private UnityEngine.Experimental.Input.InputAction m_game_drop;
        private UnityEngine.Experimental.Input.InputAction m_game_throwmail;
        private UnityEngine.Experimental.Input.InputAction m_game_hover;
        private UnityEngine.Experimental.Input.InputAction m_game_aim;
        private UnityEngine.Experimental.Input.InputAction m_game_throwsnowball;
        public struct GameActions
        {
            private Controls m_Wrapper;
            public GameActions(Controls wrapper) { m_Wrapper = wrapper; }
            public UnityEngine.Experimental.Input.InputAction @move { get { return m_Wrapper.m_game_move; } }
            public UnityEngine.Experimental.Input.InputAction @look { get { return m_Wrapper.m_game_look; } }
            public UnityEngine.Experimental.Input.InputAction @jump { get { return m_Wrapper.m_game_jump; } }
            public UnityEngine.Experimental.Input.InputAction @pause { get { return m_Wrapper.m_game_pause; } }
            public UnityEngine.Experimental.Input.InputAction @grab { get { return m_Wrapper.m_game_grab; } }
            public UnityEngine.Experimental.Input.InputAction @drop { get { return m_Wrapper.m_game_drop; } }
            public UnityEngine.Experimental.Input.InputAction @throwmail { get { return m_Wrapper.m_game_throwmail; } }
            public UnityEngine.Experimental.Input.InputAction @hover { get { return m_Wrapper.m_game_hover; } }
            public UnityEngine.Experimental.Input.InputAction @aim { get { return m_Wrapper.m_game_aim; } }
            public UnityEngine.Experimental.Input.InputAction @throwsnowball { get { return m_Wrapper.m_game_throwsnowball; } }
            public UnityEngine.Experimental.Input.InputActionMap Get() { return m_Wrapper.m_game; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public UnityEngine.Experimental.Input.InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator UnityEngine.Experimental.Input.InputActionMap(GameActions set) { return set.Get(); }
        }
        public GameActions @game
        {
            get
            {
                if (!m_Initialized) Initialize();
                return new GameActions(this);
            }
        }
    }
}
