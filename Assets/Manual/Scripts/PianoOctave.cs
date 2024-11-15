using System;
using UnityEngine;

public class PianoOctave : MonoBehaviour {
  [SerializeField] protected Key[] keys;
  [SerializeField] protected int startKey = 0;

  public void SetKeyValue(float[] values) {
    if (values.Length != keys.Length) {
      throw new ArgumentException("values length must be equal to keys length");
    }
    for (var i = 0; i < keys.Length; i++) {
      keys[i].SetTargetFrame(values[i]);
    }
  }
  public void SetSingleKeyValue(int index, float value) {
    keys[index].SetTargetFrame(value);
  }
  
  public void SetSingleKeyShouldBePressed(int index, bool shouldBePressed) {
    keys[index].SetShouldBePressed(shouldBePressed);
  }
  
  public void RegisterManager(PianoManager manager, int startKey) {
    for (var i = 0; i < keys.Length; i++) {
      keys[i].pianoManager = manager;
      keys[i].Events = manager.GetEventsFor(startKey + i);
    }
  }
}