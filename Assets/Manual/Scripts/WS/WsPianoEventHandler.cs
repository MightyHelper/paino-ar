using System;
using System.Collections.Generic;
using System.Linq;
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

  private Logger _logger;

  private void Awake() {
    _logger = GetComponent<Logger>();
  }

  /// <summary>
  /// Set the key value of the piano
  /// </summary>
  /// <param name="evt">The event to set the key value</param>
  public override void OnWsEvent(WsEvent evt) {
    if (evt.Payload.Key <= 0) return;
    pianos.ForEach(piano => piano.SetSingleKeyValue(evt.Payload.Key, evt.Payload.Type == "noteOn" ? 1f : 0f));
  }

  /// <summary>
  /// Add a piano to the list of pianos 
  /// </summary>
  /// <param name="go">The piano to add</param>
  public void AddPiano(GameObject go) {
    pianos.Add(go.GetComponent<Piano>());
    _logger.Log($"Added piano {go.name}; now {pianos.Count} pianos.");
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