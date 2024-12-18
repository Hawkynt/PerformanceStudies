using System.Runtime.CompilerServices;
using Corlib;

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
      if (!OpCodes.IsEqual(OpCodes.LoadQWord(source), comparison))
        return false;

    // Compare in 4-byte chunks using uint
    if (count >= 4) {
      if (!OpCodes.IsEqual(OpCodes.LoadDWord(source), comparison))
        return false;

      source += 4;
      comparison += 4;
      count -= 4;
    }

    // Compare in 2-byte chunks using ushort
    if (count >= 2) {
      if (!OpCodes.IsEqual(OpCodes.LoadWord(source), comparison))
        return false;

      source += 2;
      comparison += 2;
      count -= 2;
    }

    // Handle remaining byte, if any
    return count <= 0 || OpCodes.IsEqual(OpCodes.LoadByte(source), comparison);
  }
  
}