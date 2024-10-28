using System;
using UnityEngine;

public class PianoOctave : MonoBehaviour {
  [SerializeField] protected Key[] keys;

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
}