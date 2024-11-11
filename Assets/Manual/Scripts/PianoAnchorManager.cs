using UnityEngine;

public class PianoAnchorManager: AnchorManager {
  public WsPianoEventHandler wsPianoEventHandler;

  protected override void Awake() {
    base.Awake();
    _logger.Log("Piano anchor manager awake.");
  }

  protected override void OnNewItem(GameObject go) {
    _logger.Log($"Adding piano {go.name}...");
    wsPianoEventHandler.AddPiano(go);
    base.OnNewItem(go);
  }
  
  protected override void OnItemRemoved(GameObject go) {
    _logger.Log($"Removing piano {go.name}...");
    wsPianoEventHandler.RemovePiano(go);
    base.OnItemRemoved(go);
  }
}
