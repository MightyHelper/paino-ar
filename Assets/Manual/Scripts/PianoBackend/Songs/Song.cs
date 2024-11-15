using System;
using WS;

namespace PianoBackend.Songs {
  [Serializable]
  public abstract class Song {
    public KeyEvent[] KeyEvents;
    protected abstract KeyEvent[] GetKeyEvents();

    protected Song() {
      // ReSharper disable once VirtualMemberCallInConstructor
      KeyEvents = GetKeyEvents();
    }
  }
}