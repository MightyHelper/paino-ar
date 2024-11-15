using UnityEngine;

public abstract class InputContext : MonoBehaviour {
  private bool _isActive;
  public bool IsActive => _isActive;

  public virtual void Activate() {
    _isActive = true;
  }

  public virtual void Deactivate() {
    _isActive = false;
  }

  public abstract void OnKey(OVRInput.Button button);
}