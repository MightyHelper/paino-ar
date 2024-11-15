using System;

[Serializable]
public class InputManagerContextEntry {
  public InputContext context;
  public OVRInput.Button button;

  public InputManagerContextEntry(InputContext context, OVRInput.Button button) {
    this.context = context;
    this.button = button;
  }
}