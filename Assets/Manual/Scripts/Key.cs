using UnityEngine;

public class Key : MonoBehaviour {
  public float downDistance = 20f;
  public float downAngle = 7f;
  private float _targetFrame;
  private float _currentFrame;
  
  public void SetTargetFrame(float frame) {
    _targetFrame = frame;
  }

  private void FixedUpdate() {
    var vector3 = transform.localPosition;
    var rotation = transform.localEulerAngles;
    vector3.y = Mathf.Lerp(0, -downDistance / (24 * 24), _currentFrame);
    rotation.z = Mathf.Lerp(0, -downAngle, _currentFrame);
    transform.localPosition = vector3;
    transform.localEulerAngles = rotation;
    _currentFrame = Mathf.Lerp(_currentFrame, _targetFrame, 0.9f);
  }
}