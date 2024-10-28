using System;
using Meta.Net.NativeWebSocket;
using OVRSimpleJSON;
using UnityEngine;

public class WebSocketConnection : MonoBehaviour {
  public string host = "192.168.3.2";
  public int port = 6443;
  [SerializeField] protected WsEventHandler[] wsEventHandlers;
  private WebSocket _websocket;
  private Logger _logger;
  private string URL => $"ws://{host}:{port}";

  public void Awake() {
    _logger = GetComponent<Logger>();
  }

  public async void Start() {
    _websocket = new WebSocket(URL);
    _websocket.OnOpen += () => { _logger.Log("ws:Connection open!"); };
    _websocket.OnError += e => { _logger.Log("ws:Error! " + e); };
    _websocket.OnClose += e => { _logger.Log("ws:Connection closed!"); };
    _websocket.OnMessage += bytes => {
      try {
        var jsonNode = JSON.Parse(System.Text.Encoding.UTF8.GetString(bytes));
        var wsEvent = new WsEvent(jsonNode);
        if (wsEvent.Kind == "keys") {
          foreach (var handler in wsEventHandlers) {
            handler.OnWsEvent(wsEvent);
          }
        } else {
          _logger.Log("ws:Unknown event kind: " + wsEvent.Kind);
        }
      } catch (Exception e) {
        _logger.Log("ws:Error parsing JSON! " + e);
      }
    };

    _logger.Log($"ws:Attempting Connection to {URL}");
    // waiting for messages
    await _websocket.Connect();
  }


  public void Update() {
#if !UNITY_WEBGL || UNITY_EDITOR
    _websocket.DispatchMessageQueue();
#endif
  }

  public async void SendWebSocketMessage() {
    if (_websocket.State != WebSocketState.Open) return;
    // Sending bytes
    await _websocket.Send(new byte[] { 10, 20, 30 });

    // Sending plain text
    await _websocket.SendText("plain text message");
  }

  private async void OnApplicationQuit() {
    await _websocket.Close();
  }
}