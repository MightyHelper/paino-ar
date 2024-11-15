using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using JetBrains.Annotations;
using PianoBackend.Songs;
using UnityEngine;
using WS;

namespace PianoBackend {
  /// <summary>
  /// Manages the current song and notifies listeners when the song changes. 
  /// </summary>
  public class PianoManager : InputContext {
    public Logger logger;
    public SongItem[] songItems;
    private List<ISongUpdateListener> _songUpdateListeners = new();

    [CanBeNull] private Song _currentSong;

    private float _currentTime;
    public float CurrentTime => _currentTime;

    public KeyEvent[] GetEventsFor(int index) {
      return _currentSong?.KeyEvents?.Where(keyEvent => keyEvent.Key == index).ToArray() ?? Array.Empty<KeyEvent>();
    }

    public KeyEvent[] GetCurrentTimeEvents() {
      return _currentSong?.KeyEvents
               ?.Where(keyEvent => Mathf.Approximately(keyEvent.Start, CurrentTime) && !keyEvent.Done).ToArray() ??
             Array.Empty<KeyEvent>();
    }

    public void OnKey(int key, bool noteOn) {
      if (!noteOn) return;
      var remainingEvents = GetCurrentTimeEvents();
      foreach (var keyEvent in remainingEvents) {
        if (keyEvent.Key != key) continue;
        keyEvent.Done = true;
        break;
      }

      var remainingCount = remainingEvents.Count(keyEvent => !keyEvent.Done);
      if (remainingCount != 0) return;
      _currentTime += 1;
    }

    public override void Activate() {
      base.Activate();
      logger.Log("PianoManager: Activate.");
      songItems.ToList().ForEach(
        songItem => { logger.Log($"PianoManager: Song {songItem.button} {songItem.songType}."); }
      );
    }

    private static Song LoadSongType(SongType type) {
      return type switch {
        SongType.OdeToJoy => new OdeToJoy(),
        SongType.FurElise => new FurElise(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
      };
    }

    public override void OnKey(OVRInput.Button button) {
      logger.Log($"PianoManager: Button {button} pressed.");
      var songItem = songItems.FirstOrDefault(item => item.button == button);
      if (songItem == null) return;
      ChangeCurrentSong(songItem);
    }

    private void ChangeCurrentSong(SongItem songItem) {
      logger.Log($"Switching to Song {songItem.songType}.");
      logger.Log($"And Notifying {_songUpdateListeners.Count} listeners.");
      _currentSong = LoadSongType(songItem.songType);
      _currentTime = 0;
      _songUpdateListeners.ForEach(
        listener => {
          logger.Log($"Notifying listener {listener}");
          listener.OnSongUpdate(this);
        }
      );
    }

    public void RegisterSongUpdateListener(ISongUpdateListener listener) {
      _songUpdateListeners.Add(listener);
    }

    public void RemoveSongUpdateListener(ISongUpdateListener listener) {
      _songUpdateListeners.Remove(listener);
    }
  }
}