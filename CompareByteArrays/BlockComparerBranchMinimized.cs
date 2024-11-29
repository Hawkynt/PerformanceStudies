using System.Runtime.CompilerServices;

namespace Classes;

internal static unsafe class BlockComparerBranchMinimized {
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

      var result = *source ^ *comparison;
      result |= source[1] ^ comparison[1];
      result |= source[2] ^ comparison[2];
      result |= source[3] ^ comparison[3];
      result |= source[4] ^ comparison[4];
      result |= source[5] ^ comparison[5];
      result |= source[6] ^ comparison[6];
      result |= source[7] ^ comparison[7];
      if (result != 0)
        return false;

      source += 8;
      comparison += 8;
      count -= 8;
    }

    // Handle remaining bytes
    for (; count > 0; ++source, ++comparison, --count)
      if (*source != *comparison)
        return false;

    return true;
  }


}