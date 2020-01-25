namespace ProjectA.Interfaces {
    using UnityEngine;
    public interface IListener {
        void OnEvent(EVENT_TYPE event_type, Component sender, object param);    
    }
}
