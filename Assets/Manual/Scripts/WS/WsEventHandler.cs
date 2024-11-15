using UnityEngine;

namespace WS {
    /// <summary>
    /// The WebSocket event handler.
    /// </summary>
    public abstract class WsEventHandler: MonoBehaviour {
        /// <summary>
        /// This is called by the WebSocket connection when a new event is received.
        /// </summary>
        /// <param name="evt">The event to handle</param>
        public abstract void OnWsEvent(WsEvent evt);
    }
}