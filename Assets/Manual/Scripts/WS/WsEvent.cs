using OVRSimpleJSON;

public struct WsEvent {
    public string Kind;
    public WsKeyEvent Payload;

    public WsEvent(JSONNode jsonNode) {
        Kind = jsonNode["type"];
        Payload = new WsKeyEvent {
            Type = jsonNode["payload"]["type"],
            Channel = jsonNode["payload"]["channel"],
            State = jsonNode["payload"]["state"],
            Key = jsonNode["payload"]["key"],
        };
    }

    public override string ToString() {
        return $"{{ Kind: {Kind}, Payload: {Payload} }}";
    }
}