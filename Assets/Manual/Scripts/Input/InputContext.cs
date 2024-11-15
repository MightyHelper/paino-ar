using UnityEngine;

namespace Input {
  public abstract class InputContext : MonoBehaviour {
    public bool IsActive { get; private set; }

    public virtual void Activate() {
      IsActive = true;
    }

    public virtual void Deactivate() {
      IsActive = false;
    }

    public abstract void OnKey(OVRInput.Button button);
  }
}