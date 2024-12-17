using System.Runtime.CompilerServices;
using Corlib;
using word = System.Int16;
using dword = System.Int32;
using qword = System.Int64;
using dqword = System.Runtime.Intrinsics.Vector128<byte>;
using qqword = System.Runtime.Intrinsics.Vector256<byte>;
using dqqword = System.Runtime.Intrinsics.Vector512<byte>;

namespace Classes;

internal static unsafe class BlockComparerInterleaved {
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
        return source[0] == comparison[0];
      case 2:
        return source[0] == comparison[0] & source[1] == comparison[1];
      case 3: {
        dword e0 = source[0];
        dword e1 = source[1];
        dword e2 = source[2];
        
        var e3 = comparison[0];
        var e4 = comparison[1];
        var e5 = comparison[2];

        OpCodes.Compare(ref e0, e3);
        OpCodes.Compare(ref e1, e4);
        OpCodes.Compare(ref e2, e5);
        return OpCodes.IsZero(e0, e1, e2);
      }
      case 4: {
        dword e0 = source[0];
        dword e1 = source[1];
        dword e2 = source[2];
        dword e3 = source[3];

        var e4 = comparison[0];
        var e5 = comparison[1];
        var e6 = comparison[2];
        var e7 = comparison[3];

        OpCodes.Compare(ref e0, e4);
        OpCodes.Compare(ref e1, e5);
        OpCodes.Compare(ref e2, e6);
        OpCodes.Compare(ref e3, e7);
        return OpCodes.IsZero(e0, e1, e2, e3);
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
    dword e0, e1, e2,e3,e4,e5;
    qword r0, r1, r2, r3, r4, r5, r6, r7, r8;
    for (;;)
      switch (count) {
        case 0: return true;
        case 1: return *source == *comparison;
        case 2: return *(word*)source == *(word*)comparison;
        case 3: {
          e0 = *(word*)source;
          e1 = source[2];
          e2 = *(word*)comparison;
          e3 = comparison[2];
          OpCodes.Compare(ref e0, e2);
          OpCodes.Compare(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        }
        case 4: return *(dword*)source == *(dword*)comparison;
        case 5:
          e0 = *(dword*)source;
          e1 = source[4];
          e2 = *(dword*)comparison;
          e3 = comparison[4];
          OpCodes.Compare(ref e0, e2);
          OpCodes.Compare(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        case 6:
          e0 = *(dword*)source;
          e1 = ((word*)source)[2];
          e2 = *(dword*)comparison;
          e3 = ((word*)comparison)[2];
          OpCodes.Compare(ref e0, e2);
          OpCodes.Compare(ref e1, e3);
          return OpCodes.IsZero(e0,e1);
        case 7:
          e0 = *(dword*)source;
          e1 = ((word*)source)[2];
          e2 = source[6];
          e3 = *(dword*)comparison;
          e4 = ((word*)comparison)[2];
          e5 = comparison[6];
          OpCodes.Compare(ref e0, e3);
          OpCodes.Compare(ref e1, e4);
          OpCodes.Compare(ref e2, e5);
          return OpCodes.IsZero(e0, e1,e2);
        case 8 when RuntimeConfiguration.IsLongHardwareAccelerated: 
          return *(qword*)source == *(qword*)comparison;
        case 8:
          e0 = *(dword*)source;
          e1 = *(dword*)(source + 8);
          e2 = *(dword*)comparison;
          e3 = *(dword*)(comparison + 8);
          OpCodes.Compare(ref e0, e2);
          OpCodes.Compare(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        case 9:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadByte(source, 8);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadByte(comparison, 8);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 10:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadWord(source, 8);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadWord(comparison, 8);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 11:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadWord(source, 8);
          r2 = OpCodes.QWLoadByte(source, 10);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadWord(comparison, 8);
          r6 = OpCodes.QWLoadByte(comparison, 10);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          OpCodes.Compare(ref r2, r6);
          return OpCodes.IsZero(r0, r1, r2);
        case 12:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadDWord(source, 8);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadDWord(comparison, 8);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 13:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadDWord(source, 8);
          r2 = OpCodes.QWLoadByte(source, 12);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadDWord(comparison, 8);
          r6 = OpCodes.QWLoadByte(comparison, 12);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          OpCodes.Compare(ref r2, r6);
          return OpCodes.IsZero(r0, r1,r2);
        case 14:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadDWord(source, 8);
          r2 = OpCodes.QWLoadWord(source, 12);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadDWord(comparison, 8);
          r6 = OpCodes.QWLoadWord(comparison, 12);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          OpCodes.Compare(ref r2, r6);
          return OpCodes.IsZero(r0, r1, r2);
        case 15:
          r0 = OpCodes.QWLoadQWord(source, 0);
          r1 = OpCodes.QWLoadDWord(source, 8);
          r2 = OpCodes.QWLoadWord(source, 12);
          r3 = OpCodes.QWLoadByte(source, 14);
          r4 = OpCodes.QWLoadQWord(comparison, 0);
          r5 = OpCodes.QWLoadDWord(comparison, 8);
          r6 = OpCodes.QWLoadWord(comparison, 12);
          r7 = OpCodes.QWLoadByte(comparison, 14);
          OpCodes.Compare(ref r0, r4);
          OpCodes.Compare(ref r1, r5);
          OpCodes.Compare(ref r2, r6);
          OpCodes.Compare(ref r3, r7);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 16:
          return OpCodes.LoadAndCompareDQWordB(source, comparison, 0);
        case 17:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadByte(source, 16);
          r4 = OpCodes.QWLoadByte(comparison, 16);
          OpCodes.Compare(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 18:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadWord(source, 16);
          r4 = OpCodes.QWLoadWord(comparison, 16);
          OpCodes.Compare(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 19:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadWord(source, 16);
          r2 = OpCodes.QWLoadByte(source, 18);
          r4 = OpCodes.QWLoadWord(comparison, 16);
          r5 = OpCodes.QWLoadByte(comparison, 18);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1,r2);
        case 20:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadDWord(source, 16);
          r4 = OpCodes.QWLoadDWord(comparison, 16);
          OpCodes.Compare(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 21:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadDWord(source, 16);
          r2 = OpCodes.QWLoadByte(source, 20);
          r4 = OpCodes.QWLoadDWord(comparison, 16);
          r5 = OpCodes.QWLoadByte(comparison, 20);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 22:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadDWord(source, 16);
          r2 = OpCodes.QWLoadWord(source, 20);
          r4 = OpCodes.QWLoadDWord(comparison, 16);
          r5 = OpCodes.QWLoadWord(comparison, 20);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 23:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadDWord(source, 16);
          r2 = OpCodes.QWLoadWord(source, 20);
          r3 = OpCodes.QWLoadByte(source, 22);
          r4 = OpCodes.QWLoadDWord(comparison, 16);
          r5 = OpCodes.QWLoadWord(comparison, 20);
          r6 = OpCodes.QWLoadByte(comparison, 22);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          OpCodes.Compare(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 24:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          OpCodes.Compare(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 25:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadByte(source, 24);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadByte(comparison, 24);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 26:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadWord(source, 24);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadWord(comparison, 24);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 27:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadWord(source, 24);
          r3 = OpCodes.QWLoadByte(source, 26);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadWord(comparison, 24);
          r6 = OpCodes.QWLoadByte(comparison, 26);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          OpCodes.Compare(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 28:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadDWord(source, 24);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadDWord(comparison, 24);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 29:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadDWord(source, 24);
          r3 = OpCodes.QWLoadByte(source, 28);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadDWord(comparison, 24);
          r6 = OpCodes.QWLoadByte(comparison, 28);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          OpCodes.Compare(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 30:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadDWord(source, 24);
          r3 = OpCodes.QWLoadWord(source, 28);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadDWord(comparison, 24);
          r6 = OpCodes.QWLoadWord(comparison, 28);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          OpCodes.Compare(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 31:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.QWLoadQWord(source, 16);
          r2 = OpCodes.QWLoadDWord(source, 24);
          r3 = OpCodes.QWLoadWord(source, 28);
          r8 = OpCodes.QWLoadByte(source, 30);
          r4 = OpCodes.QWLoadQWord(comparison, 16);
          r5 = OpCodes.QWLoadDWord(comparison, 24);
          r6 = OpCodes.QWLoadWord(comparison, 28);
          r7 = OpCodes.QWLoadByte(comparison, 30);
          OpCodes.Compare(ref r1, r4);
          OpCodes.Compare(ref r2, r5);
          OpCodes.Compare(ref r3, r6);
          OpCodes.Compare(ref r8, r7);
          return OpCodes.IsZero(r0 | r8, r1, r2, r3);
        case 32:
          return OpCodes.LoadAndCompareQQWordB(source, comparison, 0);
        case 64:
          return OpCodes.LoadAndCompareDQQWordB(source, comparison, 0);
        default:

          for (; count >= 48;) {
            
            var x0 = OpCodes.DQWLoadDQWord(source, 0);
            var x1 = OpCodes.DQWLoadDQWord(source, 16);
            var x3 = OpCodes.DQWLoadDQWord(comparison, 0);
            var x4 = OpCodes.DQWLoadDQWord(comparison, 16);
            OpCodes.Compare(ref x0, x3);

            var x2 = OpCodes.DQWLoadDQWord(source, 32);
            var x5 = OpCodes.DQWLoadDQWord(comparison, 32);
            OpCodes.Compare(ref x1, x4);
            OpCodes.Compare(ref x2, x5);

            if (!OpCodes.IsZero(x0, x1, x2))
              return false;

            source += 48;
            comparison += 48;
            count -= 48;
          }

          // Compare a 32-byte chunk
          if (count >= 32) {
            if (!OpCodes.LoadAndCompareQQWordB(source, comparison, 0))
              return false;

            source += 32;
            comparison += 32;
            count -= 32;
          }

          // less than 32 bytes, use the "perfect" case
          break;
      }

  }

}