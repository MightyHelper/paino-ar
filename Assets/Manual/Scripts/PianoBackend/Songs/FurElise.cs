using System;
using WS;

namespace PianoBackend.Songs {
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
}