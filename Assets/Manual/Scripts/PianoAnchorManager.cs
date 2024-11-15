using UnityEngine;

public class PianoAnchorManager: AnchorManager {
  public WsPianoEventHandler wsPianoEventHandler;

  protected override void Awake() {
    base.Awake();
    logger.Log("Piano anchor manager awake.");
  }

  protected override void OnNewItem(GameObject go) {
    logger.Log($"Adding piano {go.name}...");
    wsPianoEventHandler.AddPiano(go);
    base.OnNewItem(go);
  }

  protected override void OnItemRemoved(GameObject go) {
    logger.Log($"Removing piano {go.name}...");
    wsPianoEventHandler.RemovePiano(go);
    base.OnItemRemoved(go);
  }

  public override void Activate() {
    base.Activate();
    logger.Log("Piano anchor manager activated.");
  }
}
