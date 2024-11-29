using System.Runtime.CompilerServices;

namespace Classes;

internal static unsafe class BlockComparerUnrolled {
  /// <summary>
  /// Compares two byte-arrays starting from their beginnings.
  /// </summary>
  /// <param name="source">The source.</param>
  /// <param name="sourceLength">Length of the source.</param>
  /// <param name="comparison">The comparison.</param>
  /// <param name="comparisonLength">Length of the comparison.</param>
  /// <returns><c>true</c> if both arrays contain the same data; otherwise, <c>false</c>.</returns>
  public static bool IsEqual(byte[] source, int sourceLength, byte[] comparison, int comparisonLength) {
    if (sourceLength != comparisonLength)
      return false;

    if (ReferenceEquals(source, comparison) || sourceLength <= 0)
      return true;

    fixed (byte* sourcePin = source, comparisonPin = comparison)
      return
        _CompareBytes(sourcePin, comparisonPin, ref sourceLength)
        ;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool _CompareBytes(byte* source, byte* comparison, ref int count) {
    while (count >= 8) {
      if (*source != *comparison ||
          source[1] != comparison[1] ||
          source[2] != comparison[2] ||
          source[3] != comparison[3] ||
          source[4] != comparison[4] ||
          source[5] != comparison[5] ||
          source[6] != comparison[6] ||
          source[7] != comparison[7])
        return false;

      source += 8;
      comparison += 8;
      count -= 8;
    }

    // Handle remaining bytes
    while (count > 0) {
      if (*source != *comparison)
        return false;

      ++source;
      ++comparison;
      --count;
    }

    return true;
  }


}