using System;
using PianoBackend.Songs;

namespace PianoBackend {
  [Serializable]
  public class SongItem {
    public SongType songType;
    public OVRInput.Button button;
  }
}