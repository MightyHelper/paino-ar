using UnityEngine;

public class Piano : MonoBehaviour {
  [SerializeField] protected PianoOctave[] octaves;
  [SerializeField] protected int startKey = 0;
  
  public void SetSingleKeyValue(int index, float value) {
    var octave = index / 12;
    var key = index % 12;
    octaves[octave].SetSingleKeyValue(key, value);
  }
  
  public void SetKeyValue(float[] values) {
    for (var i = 0; i < values.Length; i++) {
      SetSingleKeyValue(i, values[i]);
    }
  }
}