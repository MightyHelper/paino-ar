using UnityEngine;

public class PianoAnchorManager: AnchorManager {
  public WsPianoEventHandler wsPianoEventHandler;

  protected override void Awake() {
    base.Awake();
    _logger.Log("Piano anchor manager awake.");
  }

  protected override void OnNewItem(GameObject go) {
    _logger.Log($"Adding piano {go.name}...");
    base.OnNewItem(go);
    wsPianoEventHandler.AddPiano(go);
  }
  
  protected override void OnItemRemoved(GameObject go) {
    base.OnItemRemoved(go);
    _logger.Log($"Removing piano {go.name}...");
    wsPianoEventHandler.RemovePiano(go);
  }
}
