using System.Runtime.CompilerServices;

namespace Classes;

internal static unsafe class BlockComparerChunksUnrolled {
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

    // Compare in 64-byte chunks using ulong
    for (; count >= 64; source += 64, comparison += 64, count -= 64) {
      // Load from source
      var r0 = *(ulong*)source;
      var r1 = ((ulong*)source)[1];
      var r2 = ((ulong*)source)[2];
      var r3 = ((ulong*)source)[3];
      var r4 = ((ulong*)source)[4];
      var r5 = ((ulong*)source)[5];
      var r6 = ((ulong*)source)[6];
      var r7 = ((ulong*)source)[7];

      // XOR with comparison
      r0 ^= *(ulong*)comparison;
      r1 ^= ((ulong*)comparison)[1];
      r2 ^= ((ulong*)comparison)[2];
      r3 ^= ((ulong*)comparison)[3];
      r4 ^= ((ulong*)comparison)[4];
      r5 ^= ((ulong*)comparison)[5];
      r6 ^= ((ulong*)comparison)[6];
      r7 ^= ((ulong*)comparison)[7];

      // Combine results using OR in stages to reduce dependency
      r0 |= r1;
      r2 |= r3;
      r4 |= r5;
      r6 |= r7;

      r0 |= r2;
      r4 |= r6;

      var result = r0 | r4;
      if (result != 0)
        return false;
    }

    // Compare in 8-byte chunks using ulong
    for (; count >= 8; source += 8, comparison += 8, count -= 8)
      if (*(ulong*)source != *(ulong*)comparison)
        return false;

    // Compare in 4-byte chunks using uint
    for (; count >= 4; source += 4, comparison += 4, count -= 4)
      if (*(uint*)source != *(uint*)comparison)
        return false;

    // Compare in 2-byte chunks using ushort
    for (; count >= 2; source += 2, comparison += 2, count -= 2)
      if (*(ushort*)source != *(ushort*)comparison)
        return false;

    // Handle remaining bytes
    for (; count > 0; ++source, ++comparison, --count)
      if (*source != *comparison)
        return false;


    return true;
  }
  
}