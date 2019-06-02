// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Controls.inputactions'

namespace pdxpartyparrot.Core.Input
{
    [System.Serializable]
    public class Controls : UnityEngine.InputSystem.InputActionAsset
    {
        private bool m_Initialized;
        private void Initialize()
        {
            // game
            m_game = GetActionMap("game");
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
            m_game_movebackward = m_game.GetAction("move backward");
            m_game_moveleft = m_game.GetAction("move left");
            m_game_moveright = m_game.GetAction("move right");
            m_game_moveforward = m_game.GetAction("move forward");
            m_game_movedown = m_game.GetAction("move down");
            m_Initialized = true;
        }
        // game
        private UnityEngine.InputSystem.InputActionMap m_game;
        private UnityEngine.InputSystem.InputAction m_game_move;
        private UnityEngine.InputSystem.InputAction m_game_look;
        private UnityEngine.InputSystem.InputAction m_game_jump;
        private UnityEngine.InputSystem.InputAction m_game_pause;
        private UnityEngine.InputSystem.InputAction m_game_grab;
        private UnityEngine.InputSystem.InputAction m_game_drop;
        private UnityEngine.InputSystem.InputAction m_game_throwmail;
        private UnityEngine.InputSystem.InputAction m_game_hover;
        private UnityEngine.InputSystem.InputAction m_game_aim;
        private UnityEngine.InputSystem.InputAction m_game_throwsnowball;
        private UnityEngine.InputSystem.InputAction m_game_movebackward;
        private UnityEngine.InputSystem.InputAction m_game_moveleft;
        private UnityEngine.InputSystem.InputAction m_game_moveright;
        private UnityEngine.InputSystem.InputAction m_game_moveforward;
        private UnityEngine.InputSystem.InputAction m_game_movedown;
        public struct GameActions
        {
            private Controls m_Wrapper;
            public GameActions(Controls wrapper) { m_Wrapper = wrapper; }
            public UnityEngine.InputSystem.InputAction @move { get { return m_Wrapper.m_game_move; } }
            public UnityEngine.InputSystem.InputAction @look { get { return m_Wrapper.m_game_look; } }
            public UnityEngine.InputSystem.InputAction @jump { get { return m_Wrapper.m_game_jump; } }
            public UnityEngine.InputSystem.InputAction @pause { get { return m_Wrapper.m_game_pause; } }
            public UnityEngine.InputSystem.InputAction @grab { get { return m_Wrapper.m_game_grab; } }
            public UnityEngine.InputSystem.InputAction @drop { get { return m_Wrapper.m_game_drop; } }
            public UnityEngine.InputSystem.InputAction @throwmail { get { return m_Wrapper.m_game_throwmail; } }
            public UnityEngine.InputSystem.InputAction @hover { get { return m_Wrapper.m_game_hover; } }
            public UnityEngine.InputSystem.InputAction @aim { get { return m_Wrapper.m_game_aim; } }
            public UnityEngine.InputSystem.InputAction @throwsnowball { get { return m_Wrapper.m_game_throwsnowball; } }
            public UnityEngine.InputSystem.InputAction @movebackward { get { return m_Wrapper.m_game_movebackward; } }
            public UnityEngine.InputSystem.InputAction @moveleft { get { return m_Wrapper.m_game_moveleft; } }
            public UnityEngine.InputSystem.InputAction @moveright { get { return m_Wrapper.m_game_moveright; } }
            public UnityEngine.InputSystem.InputAction @moveforward { get { return m_Wrapper.m_game_moveforward; } }
            public UnityEngine.InputSystem.InputAction @movedown { get { return m_Wrapper.m_game_movedown; } }
            public UnityEngine.InputSystem.InputActionMap Get() { return m_Wrapper.m_game; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public UnityEngine.InputSystem.InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator UnityEngine.InputSystem.InputActionMap(GameActions set) { return set.Get(); }
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
