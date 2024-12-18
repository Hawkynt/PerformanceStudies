using System.Runtime.CompilerServices;
using Corlib;

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
      var r0 = OpCodes.LoadQWord(source);
      var r1 = OpCodes.LoadQWord(source, 8);
      var r2 = OpCodes.LoadQWord(source, 16);
      var r3 = OpCodes.LoadQWord(source, 24);
      var r4 = OpCodes.LoadQWord(source, 32);
      var r5 = OpCodes.LoadQWord(source, 40);
      var r6 = OpCodes.LoadQWord(source, 48);
      var r7 = OpCodes.LoadQWord(source, 56);

      // XOR with comparison
      OpCodes.Xor(ref r0, comparison);
      OpCodes.Xor(ref r1, comparison, 8);
      OpCodes.Xor(ref r2, comparison, 16);
      OpCodes.Xor(ref r3, comparison, 24);
      OpCodes.Xor(ref r4, comparison, 32);
      OpCodes.Xor(ref r5, comparison, 40);
      OpCodes.Xor(ref r6, comparison, 48);
      OpCodes.Xor(ref r7, comparison, 56);

      if (!OpCodes.IsZero(r0, r1, r2, r3, r4, r5, r6, r7))
        return false;
    }

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