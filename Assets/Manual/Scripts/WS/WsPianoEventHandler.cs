using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The WebSocket piano event handler.
///
/// This class is responsible for handling the WebSocket events for the piano.
/// </summary>
public class WsPianoEventHandler: WsEventHandler {
  /// <summary>
  /// The list of pianos
  /// </summary>
  public List<Piano> pianos = new();
  private readonly PianoManager _pianoManager = new();
  private int[] _hoveredKeys = Array.Empty<int>();
  private Logger _logger;

  private void Awake() {
    _logger = GetComponent<Logger>();
    _pianoManager._logger = _logger;
  }

  /// <summary>
  /// Set the key value of the piano
  /// </summary>
  /// <param name="evt">The event to set the key value</param>
  public override void OnWsEvent(WsEvent evt) {
    var payloadKey = evt.Payload.Key;
    var noteOn = evt.Payload.Type == "noteOn";
    if (payloadKey <= 0) return;
    _pianoManager.OnKey(payloadKey, noteOn);
    pianos.ForEach(piano => {
      piano.SetSingleKeyValue(payloadKey, noteOn ? 1f : 0f);
    });
  }

  /// <summary>
  /// Add a piano to the list of pianos 
  /// </summary>
  /// <param name="go">The piano to add</param>
  public void AddPiano(GameObject go) {
    var piano = go.GetComponent<Piano>();
    pianos.Add(piano);
    _logger.Log($"Added piano {go.name}; now {pianos.Count} pianos.");
    piano.RegisterManager(_pianoManager);
    _logger.Log($"Registered manager to piano {go.name}");

  }

  /// <summary>
  /// Remove a piano from the list of pianos
  /// </summary>
  /// <param name="go">The piano to remove</param>
  public void RemovePiano(GameObject go) {
    pianos.Remove(go.GetComponent<Piano>());
    _logger.Log($"Removed piano {go.name}; now {pianos.Count} pianos.");
  }
}