using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

enum PianoKey {
  C = 0,
  CSharp = 1,
  D = 2,
  DSharp = 3,
  E = 4,
  F = 5,
  FSharp = 6,
  G = 7,
  GSharp = 8,
  A = 9,
  ASharp = 10,
  B = 11
}

[Serializable]
public abstract class Song {
  public KeyEvent[] KeyEvents;
  protected abstract KeyEvent[] GetKeyEvents();

  protected Song() {
    // ReSharper disable once VirtualMemberCallInConstructor
    KeyEvents = GetKeyEvents();
  }
}

// Generic array concat function
public static class ArrayExtensions {
  public static T[] Concat<T>(this T[] first, params T[][] second) {
    var result = new T[first.Length + second.Sum(a => a.Length)];
    first.CopyTo(result, 0);
    var offset = first.Length;
    foreach (var array in second) {
      array.CopyTo(result, offset);
      offset += array.Length;
    }

    return result;
  }

  public static T[] Concat<T>(this T[] first, params T[] second) {
    return first.Concat(new[] { second });
  }
}

[Serializable]
public class OdeToJoy : Song {
  protected override KeyEvent[] GetKeyEvents() {
    return new KeyEvent[] {
      new(60 + (int)PianoKey.E, 0),
      new(60 + (int)PianoKey.E, 1),
      new(60 + (int)PianoKey.F, 2),
      new(60 + (int)PianoKey.G, 3),
      new(60 + (int)PianoKey.G, 4),
      new(60 + (int)PianoKey.F, 5),
      new(60 + (int)PianoKey.E, 6),
      new(60 + (int)PianoKey.D, 7),
      new(60 + (int)PianoKey.C, 8),
      new(60 + (int)PianoKey.C, 9),
      new(60 + (int)PianoKey.D, 10),
      new(60 + (int)PianoKey.E, 11),
      new(60 + (int)PianoKey.E, 12),
      new(60 + (int)PianoKey.D, 13),
      new(60 + (int)PianoKey.D, 14),
      new(60 + (int)PianoKey.E, 15),
      new(60 + (int)PianoKey.E, 16),
      new(60 + (int)PianoKey.F, 17),
      new(60 + (int)PianoKey.G, 18),
      new(60 + (int)PianoKey.G, 19),
      new(60 + (int)PianoKey.F, 20),
      new(60 + (int)PianoKey.E, 21),
      new(60 + (int)PianoKey.D, 22),
      new(60 + (int)PianoKey.C, 23),
      new(60 + (int)PianoKey.C, 24),
      new(60 + (int)PianoKey.D, 25),
      new(60 + (int)PianoKey.E, 26),
      new(60 + (int)PianoKey.D, 27),
      new(60 + (int)PianoKey.C, 28),
      new(60 + (int)PianoKey.C, 29),
      new(60 + (int)PianoKey.D, 30),
      new(60 + (int)PianoKey.D, 31),
      new(60 + (int)PianoKey.E, 32),
      new(60 + (int)PianoKey.C, 33),
      new(60 + (int)PianoKey.D, 34),
      new(60 + (int)PianoKey.E, 35),
      new(60 + (int)PianoKey.F, 36),
      new(60 + (int)PianoKey.E, 37),
      new(60 + (int)PianoKey.C, 38),
      new(60 + (int)PianoKey.D, 39),
      new(60 + (int)PianoKey.E, 40),
      new(60 + (int)PianoKey.F, 41),
      new(60 + (int)PianoKey.E, 42),
      new(60 + (int)PianoKey.D, 43),
      new(60 + (int)PianoKey.C, 44),
      new(60 + (int)PianoKey.D, 45),
      new(60 - 12 + (int)PianoKey.G, 46),
      new(60 + (int)PianoKey.E, 47),
      new(60 + (int)PianoKey.E, 48),
      new(60 + (int)PianoKey.F, 49),
      new(60 + (int)PianoKey.G, 50),
      new(60 + (int)PianoKey.G, 51),
      new(60 + (int)PianoKey.F, 52),
      new(60 + (int)PianoKey.E, 53),
      new(60 + (int)PianoKey.D, 54),
      new(60 + (int)PianoKey.C, 55),
      new(60 + (int)PianoKey.C, 56),
      new(60 + (int)PianoKey.D, 57),
      new(60 + (int)PianoKey.E, 58),
      new(60 + (int)PianoKey.D, 59),
      new(60 + (int)PianoKey.C, 60),
      new(60 + (int)PianoKey.C, 61),
      //
      new(60 - 1 * 12 + (int)PianoKey.C, 0),
      new(60 - 2 * 12 + (int)PianoKey.G, 2),
      new(60 - 1 * 12 + (int)PianoKey.C, 4),
      new(60 - 2 * 12 + (int)PianoKey.G, 6),
      new(60 - 1 * 12 + (int)PianoKey.C, 8),
      new(60 - 2 * 12 + (int)PianoKey.G, 10),
      new(60 - 1 * 12 + (int)PianoKey.C, 12),
      new(60 - 2 * 12 + (int)PianoKey.G, 14),
      new(60 - 1 * 12 + (int)PianoKey.C, 15 + 0),
      new(60 - 2 * 12 + (int)PianoKey.G, 15 + 2),
      new(60 - 1 * 12 + (int)PianoKey.C, 15 + 4),
      new(60 - 2 * 12 + (int)PianoKey.G, 15 + 6),
      new(60 - 1 * 12 + (int)PianoKey.C, 15 + 8),
      new(60 - 2 * 12 + (int)PianoKey.G, 15 + 10),
      new(60 - 2 * 12 + (int)PianoKey.G, 15 + 12),
      new(60 - 2 * 12 + (int)PianoKey.C, 15 + 14),
      //
      new(60 - 2 * 12 + (int)PianoKey.G, 1 + 15 + 14 + 0),
      new(60 - 1 * 12 + (int)PianoKey.C, 1 + 15 + 14 + 2),
      new(60 - 2 * 12 + (int)PianoKey.G, 1 + 15 + 14 + 4),
      new(60 - 1 * 12 + (int)PianoKey.C, 1 + 1 + 15 + 14 + 6),
      new(60 - 2 * 12 + (int)PianoKey.G, 1 + 1 + 15 + 14 + 8),
      new(60 - 2 * 12 + (int)PianoKey.A, 1 + 2 + 15 + 14 + 10),
      new(60 - 2 * 12 + (int)PianoKey.G, 46),
      // 
      new(60 - 1 * 12 + (int)PianoKey.C, 47 + 0),
      new(60 - 2 * 12 + (int)PianoKey.G, 47 + 2),
      new(60 - 1 * 12 + (int)PianoKey.C, 47 + 4),
      new(60 - 2 * 12 + (int)PianoKey.G, 47 + 6),
      new(60 - 1 * 12 + (int)PianoKey.C, 47 + 8),
      new(60 - 2 * 12 + (int)PianoKey.G, 47 + 10),
      new(60 - 2 * 12 + (int)PianoKey.G, 47 + 12),
      new(60 - 2 * 12 + (int)PianoKey.C, 47 + 14),
    };
  }
}

[Serializable]
public class FurElise : Song {
  protected override KeyEvent[] GetKeyEvents() {
    return new KeyEvent[] {
      new(88, 0),
      new(87, 1),
      new(88, 2),
      new(87, 3),
      new(88, 4),
      new(83, 5),
      new(86, 6),
      new(84, 7),
      new(81, 8),
      new(72, 9),
      new(76, 10),
      new(81, 11),
      new(83, 12),
      new(76, 13),
      new(80, 14),
      new(83, 15),
      new(84, 16),
      new(76, 17),
      new(88, 18),
      new(87, 19),
      new(88, 20),
      new(87, 21),
      new(88, 22),
      new(83, 23),
      new(86, 24),
      new(84, 25),
      new(81, 26),
      new(72, 27),
      new(76, 28),
      new(81, 29),
      new(83, 30),
      new(76, 31),
      new(84, 32),
      new(83, 33),
      new(81, 34),
    };
  }
}

[Serializable]
public enum SongType {
  OdeToJoy, FurElise
}

[Serializable]
public class SongItem {
  public SongType songType;
  public OVRInput.Button button;
}

public interface SongUpdateListener {
  void OnSongUpdate(PianoManager manager);
}

public class PianoManager : InputContext {
  public Logger logger;
  public SongItem[] songItems;
  private List<SongUpdateListener> _songUpdateListeners = new();

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

  private Song LoadSongType(SongType type) {
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

  public void RegisterSongUpdateListener(SongUpdateListener listener) {
    _songUpdateListeners.Add(listener);
  }

  public void RemoveSongUpdateListener(SongUpdateListener listener) {
    _songUpdateListeners.Remove(listener);
  }
}