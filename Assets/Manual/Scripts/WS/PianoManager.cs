using System.Linq;
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

public class PianoManager {
  internal Logger _logger;

  private KeyEvent[] _keyEvents = {
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
  //    private KeyEvent[] _keyEvents = {
  // 	new(60 + (int)PianoKey.E, 0),
  // 	new(60 + (int)PianoKey.E, 1),
  // 	new(60 + (int)PianoKey.E, 2),
  // 	new(60 + (int)PianoKey.E, 3),
  // 	new(60 + (int)PianoKey.E, 4),
  // 	new(60 + (int)PianoKey.E, 5),
  // 	new(60 + (int)PianoKey.E, 6),
  // 	new(60 + (int)PianoKey.E, 7),
  // 	new(60 + (int)PianoKey.E, 8),
  // 	new(60 + (int)PianoKey.E, 9),
  // 	new(60 + (int)PianoKey.E, 10),
  // 	new(60 + (int)PianoKey.E, 11),
  // 	new(60 + (int)PianoKey.E, 12),
  // 	new(60 + (int)PianoKey.E, 13),
  // 	new(60 + (int)PianoKey.E, 14),
  // 	new(60 + (int)PianoKey.B, 15),
  // 	new(60 + (int)PianoKey.B, 16),
  // 	new(60 + (int)PianoKey.B, 17),
  // 	new(60 + (int)PianoKey.B, 18),
  // 	new(60 + (int)PianoKey.B, 19),
  // 	new(60 + (int)PianoKey.B, 20),
  // 	new(60 + (int)PianoKey.B, 21),
  // 	new(60 + (int)PianoKey.B, 22),
  // 	new(60 + (int)PianoKey.B, 23),
  // 	new(60 + (int)PianoKey.B, 24),
  // 	new(60 + (int)PianoKey.B, 25),
  // 	new(60 + (int)PianoKey.B, 26),
  // 	new(60 + (int)PianoKey.B, 27),
  // 	new(60 + (int)PianoKey.C, 28),
  // 	new(60 + (int)PianoKey.C, 29),
  // };

  private float _currentTime;
  public float CurrentTime => _currentTime;

  public Logger Logger => _logger;

  public KeyEvent[] GetEventsFor(int index) {
    return _keyEvents.Where(keyEvent => keyEvent.Key == index).ToArray();
  }

  public KeyEvent[] GetCurrentTimeEvents() {
    return _keyEvents.Where(keyEvent => Mathf.Approximately(keyEvent.Start, CurrentTime) && !keyEvent.Done).ToArray();
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
}