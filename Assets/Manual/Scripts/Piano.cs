using System.Linq;
using UnityEngine;

public class Piano : MonoBehaviour {
  [SerializeField] protected PianoOctave[] octaves;
  [SerializeField] protected int startKey = 0;
  
  public void SetSingleKeyValue(int index, float value) {
    index -= startKey;
    var octave = index / 12;
    var key = index % 12;
    octaves[octave].SetSingleKeyValue(key, value);
  }
  
  public void SetKeyValue(float[] values) {
    for (var i = 0; i < values.Length; i++) {
      SetSingleKeyValue(i, values[i]);
    }
  }
  
  public void SetSingleKeyShouldBePressed(int index, bool shouldBePressed) {
    index -= startKey;
    var octave = index / 12;
    var key = index % 12;
    octaves[octave].SetSingleKeyShouldBePressed(key, shouldBePressed);
  }
  
  public void RegisterManager(PianoManager manager) {
    // register the manager to all octaves offseting the start key
    for (var i = 0; i < octaves.Length; i++) {
      octaves[i].RegisterManager(manager, startKey + i * 12);
    }    
  }
}