using System.Runtime.CompilerServices;

namespace Classes;

internal static unsafe class BlockComparerInstructionLevelParallelism {
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

      // Load from source
      var r0 = *source;
      var r1 = source[1];
      var r2 = source[2];
      var r3 = source[3];
      var r4 = source[4];
      var r5 = source[5];
      var r6 = source[6];
      var r7 = source[7];

      // XOR with comparison
      r0 ^= *comparison;
      r1 ^= comparison[1];
      r2 ^= comparison[2];
      r3 ^= comparison[3];
      r4 ^= comparison[4];
      r5 ^= comparison[5];
      r6 ^= comparison[6];
      r7 ^= comparison[7];

      // Combine results using OR in stages to reduce dependency
      r0 |= r1;
      r2 |= r3;
      r4 |= r5;
      r6 |= r7;

      r0|=r2;
      r4 |= r6;

      var result = r0 | r4;
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