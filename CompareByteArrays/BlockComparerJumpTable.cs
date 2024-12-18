using System.Runtime.CompilerServices;
using Corlib;

namespace Classes;

internal static unsafe class BlockComparerJumpTable {
  /// <summary>
  /// Compares two byte-arrays starting from their beginnings.
  /// </summary>
  /// <param name="source">The source.</param>
  /// <param name="sourceLength">Length of the source.</param>
  /// <param name="comparison">The comparison.</param>
  /// <param name="comparisonLength">Length of the comparison.</param>
  /// <returns><c>true</c> if both arrays contain the same data; otherwise, <c>false</c>.</returns>
  [SkipLocalsInit]
  public static bool IsEqual(byte[] source, int sourceLength, byte[] comparison, int comparisonLength) {
    if (sourceLength != comparisonLength)
      return false;

    if (ReferenceEquals(source, comparison) || sourceLength <= 0)
      return true;

    switch (sourceLength) {
      case 1:
        return OpCodes.IsEqual(source[0], comparison[0]);
      case 2:
        return OpCodes.IsEqual(source[0], comparison[0]) & OpCodes.IsEqual(source[1], comparison[1]);
      case 3: {
        int r0 = source[0];
        int r1 = source[1];
        int r2 = source[2];
        OpCodes.Xor(ref r0, comparison[0]);
        OpCodes.Xor(ref r1, comparison[1]);
        OpCodes.Xor(ref r2, comparison[2]);
        return OpCodes.IsZero(r0, r1, r2);
      }
      case 4: {
        int r0 = source[0];
        int r1 = source[1];
        int r2 = source[2];
        int r3 = source[3];
        OpCodes.Xor(ref r0, comparison[0]);
        OpCodes.Xor(ref r1, comparison[1]);
        OpCodes.Xor(ref r2, comparison[2]);
        OpCodes.Xor(ref r3, comparison[3]);
        return OpCodes.IsZero(r0, r1, r2, r3);
      }
      default:
        fixed (byte* sourcePin = source, comparisonPin = comparison)
          return
            _CompareBytes(sourcePin, comparisonPin, ref sourceLength)
            ;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  [SkipLocalsInit]
  private static bool _CompareBytes(byte* source, byte* comparison, ref int count) {
    switch (count) {
      case 0: return true;
      case 1: return OpCodes.IsEqual(OpCodes.LoadByte(source), comparison);
      case 2: return OpCodes.IsEqual(OpCodes.LoadWord(source), comparison);
      case 3: {
        var e0 = OpCodes.LoadWord(source);
        var e1 = OpCodes.LoadByte(source, 2);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 2);
        return OpCodes.IsZero(e0, e1);
      }
      case 4: return OpCodes.IsEqual(OpCodes.LoadDWord(source), comparison);
      case 5: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadByte(source, 4);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        return OpCodes.IsZero(e0, e1);
      }
      case 6: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadWord(source, 4);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        return OpCodes.IsZero(e0, e1);
      }
      case 7: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadWord(source, 4);
        var e2 = OpCodes.LoadByte(source, 6);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 6);
        return OpCodes.IsZero(e0, e1, e2);
      }
      case 8 when RuntimeConfiguration.IsLongHardwareAccelerated:
        return OpCodes.IsEqual(OpCodes.LoadQWord(source), comparison);
      case 8: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        return OpCodes.IsZero(e0, e1);
      }
      case 9 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadByte(source, 8);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        return OpCodes.IsZero(r0, r1);
      }
      case 9: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadByte(source, 8);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        return OpCodes.IsZero(e0, e1, e2);
      }
      case 10 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadWord(source, 8);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        return OpCodes.IsZero(r0, r1);
      }
      case 10: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadWord(source, 8);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        return OpCodes.IsZero(e0, e1, e2);
      }
      case 11 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadWord(source, 8);
        var r2 = OpCodes.LoadByte(source, 10);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        OpCodes.Xor(ref r2, comparison, 10);
        return OpCodes.IsZero(r0, r1, r2);
      }
      case 11: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadWord(source, 8);
        var e3 = OpCodes.LoadByte(source, 10);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        OpCodes.Xor(ref e3, comparison, 10);
        return OpCodes.IsZero(e0, e1, e2, e3);
      }
      case 12 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadDWord(source, 8);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        return OpCodes.IsZero(r0, r1);
      }
      case 12: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadDWord(source, 8);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        return OpCodes.IsZero(e0, e1, e2);
      }
      case 13 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadDWord(source, 8);
        var r2 = OpCodes.LoadByte(source, 12);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        OpCodes.Xor(ref r2, comparison, 12);
        return OpCodes.IsZero(r0, r1,r2);
      }
      case 13: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadDWord(source, 8);
        var e3 = OpCodes.LoadByte(source, 12);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        OpCodes.Xor(ref e3, comparison, 12);
        return OpCodes.IsZero(e0, e1, e2,e3);
      }
      case 14 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadDWord(source, 8);
        var r2 = OpCodes.LoadWord(source, 12);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        OpCodes.Xor(ref r2, comparison, 12);
        return OpCodes.IsZero(r0, r1, r2);
      }
      case 14: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadDWord(source, 8);
        var e3 = OpCodes.LoadWord(source, 12);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        OpCodes.Xor(ref e3, comparison, 12);
        return OpCodes.IsZero(e0, e1, e2, e3);
      }
      case 15 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadDWord(source, 8);
        var r2 = OpCodes.LoadWord(source, 12);
        var r3 = OpCodes.LoadByte(source, 14);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        OpCodes.Xor(ref r2, comparison, 12);
        OpCodes.Xor(ref r3, comparison, 14);
        return OpCodes.IsZero(r0, r1, r2,r3);
      }
      case 15: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadDWord(source, 8);
        var e3 = OpCodes.LoadWord(source, 12);
        var e4 = OpCodes.LoadByte(source, 14);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        OpCodes.Xor(ref e3, comparison, 12);
        OpCodes.Xor(ref e4, comparison, 14);
        return OpCodes.IsZero(e0, e1, e2, e3,e4);
      }
      case 16 when RuntimeConfiguration.IsLongHardwareAccelerated: {
        var r0 = OpCodes.LoadQWord(source);
        var r1 = OpCodes.LoadQWord(source, 8);
        OpCodes.Xor(ref r0, comparison);
        OpCodes.Xor(ref r1, comparison, 8);
        return OpCodes.IsZero(r0, r1);
      }
      case 16: {
        var e0 = OpCodes.LoadDWord(source);
        var e1 = OpCodes.LoadDWord(source, 4);
        var e2 = OpCodes.LoadDWord(source, 8);
        var e3 = OpCodes.LoadDWord(source, 12);
        OpCodes.Xor(ref e0, comparison);
        OpCodes.Xor(ref e1, comparison, 4);
        OpCodes.Xor(ref e2, comparison, 8);
        OpCodes.Xor(ref e3, comparison, 12);
        return OpCodes.IsZero(e0, e1, e2, e3);
      }
      default:
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
          
          if (!OpCodes.IsZero(r0,r1,r2,r3,r4,r5,r6,r7))
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

}