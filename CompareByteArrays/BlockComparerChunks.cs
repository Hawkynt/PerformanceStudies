using System.Runtime.CompilerServices;

namespace Classes;

internal static unsafe class BlockComparerChunks {
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