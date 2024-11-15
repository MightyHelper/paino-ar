using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Key : MonoBehaviour {
  public float downDistance = 20f;
  public float downAngle = 7f;
  public Color normalMaterial;
  public Color requiredMaterial;
  public GameObject markerPrefab;
  public Color markerColor;
  private float _targetFrame;
  private float _currentFrame;
  private bool _shouldBePressed;
  private MeshRenderer _component;
  private KeyEventMarker[] _markers;
  private KeyEvent[] _events;

  public KeyEvent[] Events {
    set {
      // Delete possibly existing markers
      
      if (_markers != null) {
        foreach (var marker in _markers) {
          if (marker.marker) {
            Destroy(marker.marker);
          }
        }
      }
      
      _markers = new KeyEventMarker[value.Length];
      for (var i = 0; i < value.Length; i++) {
        var keyEvent = value[i];
        var marker = Instantiate(markerPrefab, transform, true);
        marker.transform.localPosition = new Vector3(0, keyEvent.Start * 1 + 1, 0);
        marker.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        marker.transform.localEulerAngles = new Vector3(0, 0, 0);
        var material = marker.GetComponent<MeshRenderer>().material;
        material.color = markerColor;
        _markers[i] = new KeyEventMarker { evt = keyEvent, marker = marker, material = material };
      }

      _events = value;
    }
    get => _events;
  }

  public PianoManager pianoManager;

  private void Start() {
    _component = GetComponent<MeshRenderer>();
  }

  public void SetTargetFrame(float frame) {
    _targetFrame = frame;
  }

  public void SetShouldBePressed(bool shouldBePressed) {
    _shouldBePressed = shouldBePressed;
  }

  private void FixedUpdate() {
    var vector3 = transform.localPosition;
    var rotation = transform.localEulerAngles;
    vector3.y = Mathf.Lerp(0, -downDistance / 24, _currentFrame);
    rotation.z = Mathf.Lerp(0, -downAngle, _currentFrame);
    _component.material.color = _shouldBePressed ? requiredMaterial : normalMaterial;
    transform.localPosition = vector3;
    transform.localEulerAngles = rotation;
    _currentFrame = Mathf.Lerp(_currentFrame, _targetFrame, 0.9f);
  }

  private void Update() {
    if (Events == null) return;
    var currentTime = pianoManager.CurrentTime;
    SetShouldBePressed(false);
    // Change Y position of markers based on time
    foreach (var marker in _markers) {
      var keyEvent = marker.evt;
      if (keyEvent.Done) {
        if (marker.marker) {
          Destroy(marker.marker);
          marker.marker = null;
        }

        continue;
      }

      var shouldBePressedNext = Mathf.Approximately(keyEvent.Start, currentTime);
      if (shouldBePressedNext) {
        SetShouldBePressed(true);
      }
      var markerY = Mathf.Lerp(marker.marker.transform.localPosition.y, keyEvent.Start * 1 - currentTime + 1, 0.1f);
      marker.marker.transform.localPosition = new Vector3(0, markerY, 0);
    }
  }
}

class KeyEventMarker {
  public KeyEvent evt;
  public GameObject marker;
  public Material material;
}