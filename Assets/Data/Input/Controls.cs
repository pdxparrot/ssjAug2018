// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Controls.inputactions'

namespace com.pdxpartyparrot.ssjAug2018
{
    [System.Serializable]
    public class Controls : UnityEngine.Experimental.Input.InputActionWrapper
    {
        private bool m_Initialized;
        private void Initialize()
        {
            // default
            m_default = asset.GetActionMap("default");
            m_Initialized = true;
        }
        // default
        private UnityEngine.Experimental.Input.InputActionMap m_default;
        public struct DefaultActions
        {
            private Controls m_Wrapper;
            public DefaultActions(Controls wrapper) { m_Wrapper = wrapper; }
            public UnityEngine.Experimental.Input.InputActionMap Get() { return m_Wrapper.m_default; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public UnityEngine.Experimental.Input.InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator UnityEngine.Experimental.Input.InputActionMap(DefaultActions set) { return set.Get(); }
        }
        public DefaultActions @default
        {
            get
            {
                if (!m_Initialized) Initialize();
                return new DefaultActions(this);
            }
        }
    }
}
