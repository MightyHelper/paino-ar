using System.Collections.Generic;
using PianoBackend;
using PianoFrontend;
using UnityEngine;

namespace WS {
  /// <summary>
  /// The WebSocket piano event handler.
  ///
  /// This class is responsible for handling the WebSocket events for the piano.
  /// </summary>
  public class WsPianoEventHandler: WsEventHandler, ISongUpdateListener {
    /// <summary>
    /// The list of pianos
    /// </summary>
    public List<Piano> pianos = new();
    public PianoManager pianoManager;
    public Logger logger;

    private void Start() {
      pianoManager.RegisterSongUpdateListener(this);
      logger.Log("Registered song update listener.");
    }

    /// <summary>
    /// Set the key value of the piano
    /// </summary>
    /// <param name="evt">The event to set the key value</param>
    public override void OnWsEvent(WsEvent evt) {
      var payloadKey = evt.Payload.Key;
      var noteOn = evt.Payload.Type == "noteOn";
      if (payloadKey <= 0) return;
      pianoManager.OnKey(payloadKey, noteOn);
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
      logger.Log($"Added piano {go.name}; now {pianos.Count} pianos.");
      piano.RegisterManager(pianoManager);
      logger.Log($"Registered manager to piano {go.name}");

    }

    /// <summary>
    /// Remove a piano from the list of pianos
    /// </summary>
    /// <param name="go">The piano to remove</param>
    public void RemovePiano(GameObject go) {
      var piano = go.GetComponent<Piano>();
      pianos.Remove(piano);
      logger.Log($"Removed piano {go.name}; now {pianos.Count} pianos.");
    }

    public void OnSongUpdate(PianoManager manager) {
      logger.Log("Song update!!!");
      pianos.ForEach(piano => {
        piano.RegisterManager(pianoManager);
      });
    }
  }
}