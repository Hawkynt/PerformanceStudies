using System.Runtime.CompilerServices;
using Corlib;

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
      var r0 = OpCodes.DWLoadByte(source);
      var r1 = OpCodes.DWLoadByte(source, 1);
      var r2 = OpCodes.DWLoadByte(source, 2);
      var r3 = OpCodes.DWLoadByte(source, 3);
      var r4 = OpCodes.DWLoadByte(source, 4);
      var r5 = OpCodes.DWLoadByte(source, 5);
      var r6 = OpCodes.DWLoadByte(source, 6);
      var r7 = OpCodes.DWLoadByte(source, 7);

      // XOR with comparison
      OpCodes.Compare(ref r0, OpCodes.LoadByte(comparison));
      OpCodes.Compare(ref r1, OpCodes.LoadByte(comparison, 1));
      OpCodes.Compare(ref r2, OpCodes.LoadByte(comparison, 2));
      OpCodes.Compare(ref r3, OpCodes.LoadByte(comparison, 3));
      OpCodes.Compare(ref r4, OpCodes.LoadByte(comparison, 4));
      OpCodes.Compare(ref r5, OpCodes.LoadByte(comparison, 5));
      OpCodes.Compare(ref r6, OpCodes.LoadByte(comparison, 6));
      OpCodes.Compare(ref r7, OpCodes.LoadByte(comparison, 7));

      if (!OpCodes.IsZero(r0, r1, r2, r3, r4, r5, r6, r7))
        return false;

      source += 8;
      comparison += 8;
      count -= 8;
    }

    // Handle remaining bytes
    for (; count > 0; ++source, ++comparison, --count)
      if (!OpCodes.IsEqual(OpCodes.LoadByte(source), comparison))
        return false;

    return true;
  }


}