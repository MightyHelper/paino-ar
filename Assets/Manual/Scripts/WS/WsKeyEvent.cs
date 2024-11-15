namespace WS {
  /// <summary>
  /// The key event, which contains the type, channel, key, and state.
  ///
  /// This represents a MIDI event. 
  /// </summary>
  public struct WsKeyEvent {
    // { type: 'noteOff', channel: 0, key: 81, state: 64 }
    public string Type;
    public int Channel;
    public int Key;
    public bool Pedal;
    public int State;
  }
}