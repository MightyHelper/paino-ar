using System.Linq;

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