using System.Runtime.CompilerServices;
using CompareByteArrays;
using word = System.Int16;
using dword = System.Int32;
using qword = System.Int64;
using dqword = System.Runtime.Intrinsics.Vector128<byte>;
using qqword = System.Runtime.Intrinsics.Vector256<byte>;
using dqqword = System.Runtime.Intrinsics.Vector512<byte>;

namespace Classes;

internal static unsafe class BlockComparerVector {
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

        OpCodes.CompareDWord(ref e0, e3);
        OpCodes.CompareDWord(ref e1, e4);
        OpCodes.CompareDWord(ref e2, e5);
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

        OpCodes.CompareDWord(ref e0, e4);
        OpCodes.CompareDWord(ref e1, e5);
        OpCodes.CompareDWord(ref e2, e6);
        OpCodes.CompareDWord(ref e3, e7);
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
          OpCodes.CompareDWord(ref e0, e2);
          OpCodes.CompareDWord(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        }
        case 4: return *(dword*)source == *(dword*)comparison;
        case 5:
          e0 = *(dword*)source;
          e1 = source[4];
          e2 = *(dword*)comparison;
          e3 = comparison[4];
          OpCodes.CompareDWord(ref e0, e2);
          OpCodes.CompareDWord(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        case 6:
          e0 = *(dword*)source;
          e1 = ((word*)source)[2];
          e2 = *(dword*)comparison;
          e3 = ((word*)comparison)[2];
          OpCodes.CompareDWord(ref e0, e2);
          OpCodes.CompareDWord(ref e1, e3);
          return OpCodes.IsZero(e0,e1);
        case 7:
          e0 = *(dword*)source;
          e1 = ((word*)source)[2];
          e2 = source[6];
          e3 = *(dword*)comparison;
          e4 = ((word*)comparison)[2];
          e5 = comparison[6];
          OpCodes.CompareDWord(ref e0, e3);
          OpCodes.CompareDWord(ref e1, e4);
          OpCodes.CompareDWord(ref e2, e5);
          return OpCodes.IsZero(e0, e1,e2);
        case 8 when RuntimeConfiguration.IsLongHardwareAccelerated: 
          return *(qword*)source == *(qword*)comparison;
        case 8:
          e0 = *(dword*)source;
          e1 = *(dword*)(source + 8);
          e2 = *(dword*)comparison;
          e3 = *(dword*)(comparison + 8);
          OpCodes.CompareDWord(ref e0, e2);
          OpCodes.CompareDWord(ref e1, e3);
          return OpCodes.IsZero(e0, e1);
        case 9:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadByte(source, 8);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadByte(comparison, 8);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 10:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadWord(source, 8);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadWord(comparison, 8);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 11:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadWord(source, 8);
          r2 = OpCodes.LoadByte(source, 10);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadWord(comparison, 8);
          r6 = OpCodes.LoadByte(comparison, 10);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          OpCodes.CompareQWord(ref r2, r6);
          return OpCodes.IsZero(r0, r1, r2);
        case 12:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadDWord(source, 8);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadDWord(comparison, 8);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          return OpCodes.IsZero(r0, r1);
        case 13:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadDWord(source, 8);
          r2 = OpCodes.LoadByte(source, 12);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadDWord(comparison, 8);
          r6 = OpCodes.LoadByte(comparison, 12);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          OpCodes.CompareQWord(ref r2, r6);
          return OpCodes.IsZero(r0, r1,r2);
        case 14:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadDWord(source, 8);
          r2 = OpCodes.LoadWord(source, 12);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadDWord(comparison, 8);
          r6 = OpCodes.LoadWord(comparison, 12);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          OpCodes.CompareQWord(ref r2, r6);
          return OpCodes.IsZero(r0, r1, r2);
        case 15:
          r0 = OpCodes.LoadQWord(source, 0);
          r1 = OpCodes.LoadDWord(source, 8);
          r2 = OpCodes.LoadWord(source, 12);
          r3 = OpCodes.LoadByte(source, 14);
          r4 = OpCodes.LoadQWord(comparison, 0);
          r5 = OpCodes.LoadDWord(comparison, 8);
          r6 = OpCodes.LoadWord(comparison, 12);
          r7 = OpCodes.LoadByte(comparison, 14);
          OpCodes.CompareQWord(ref r0, r4);
          OpCodes.CompareQWord(ref r1, r5);
          OpCodes.CompareQWord(ref r2, r6);
          OpCodes.CompareQWord(ref r3, r7);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 16:
          return OpCodes.LoadAndCompareDQWordB(source, comparison, 0);
        case 17:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadByte(source, 16);
          r4 = OpCodes.LoadByte(comparison, 16);
          OpCodes.CompareQWord(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 18:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadWord(source, 16);
          r4 = OpCodes.LoadWord(comparison, 16);
          OpCodes.CompareQWord(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 19:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadWord(source, 16);
          r2 = OpCodes.LoadByte(source, 18);
          r4 = OpCodes.LoadWord(comparison, 16);
          r5 = OpCodes.LoadByte(comparison, 18);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1,r2);
        case 20:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadDWord(source, 16);
          r4 = OpCodes.LoadDWord(comparison, 16);
          OpCodes.CompareQWord(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 21:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadDWord(source, 16);
          r2 = OpCodes.LoadByte(source, 20);
          r4 = OpCodes.LoadDWord(comparison, 16);
          r5 = OpCodes.LoadByte(comparison, 20);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 22:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadDWord(source, 16);
          r2 = OpCodes.LoadWord(source, 20);
          r4 = OpCodes.LoadDWord(comparison, 16);
          r5 = OpCodes.LoadWord(comparison, 20);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 23:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadDWord(source, 16);
          r2 = OpCodes.LoadWord(source, 20);
          r3 = OpCodes.LoadByte(source, 22);
          r4 = OpCodes.LoadDWord(comparison, 16);
          r5 = OpCodes.LoadWord(comparison, 20);
          r6 = OpCodes.LoadByte(comparison, 22);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          OpCodes.CompareQWord(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 24:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r4 = OpCodes.LoadQWord(comparison, 16);
          OpCodes.CompareQWord(ref r1, r4);
          return OpCodes.IsZero(r0, r1);
        case 25:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadByte(source, 24);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadByte(comparison, 24);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 26:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadWord(source, 24);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadWord(comparison, 24);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 27:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadWord(source, 24);
          r3 = OpCodes.LoadByte(source, 26);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadWord(comparison, 24);
          r6 = OpCodes.LoadByte(comparison, 26);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          OpCodes.CompareQWord(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 28:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadDWord(source, 24);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadDWord(comparison, 24);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          return OpCodes.IsZero(r0, r1, r2);
        case 29:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadDWord(source, 24);
          r3 = OpCodes.LoadByte(source, 28);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadDWord(comparison, 24);
          r6 = OpCodes.LoadByte(comparison, 28);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          OpCodes.CompareQWord(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 30:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadDWord(source, 24);
          r3 = OpCodes.LoadWord(source, 28);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadDWord(comparison, 24);
          r6 = OpCodes.LoadWord(comparison, 28);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          OpCodes.CompareQWord(ref r3, r6);
          return OpCodes.IsZero(r0, r1, r2, r3);
        case 31:
          r0 = OpCodes.LoadAndCompareDQWord(source, comparison, 0);
          r1 = OpCodes.LoadQWord(source, 16);
          r2 = OpCodes.LoadDWord(source, 24);
          r3 = OpCodes.LoadWord(source, 28);
          r8 = OpCodes.LoadByte(source, 30);
          r4 = OpCodes.LoadQWord(comparison, 16);
          r5 = OpCodes.LoadDWord(comparison, 24);
          r6 = OpCodes.LoadWord(comparison, 28);
          r7 = OpCodes.LoadByte(comparison, 30);
          OpCodes.CompareQWord(ref r1, r4);
          OpCodes.CompareQWord(ref r2, r5);
          OpCodes.CompareQWord(ref r3, r6);
          OpCodes.CompareQWord(ref r8, r7);
          return OpCodes.IsZero(r0 | r8, r1, r2, r3);
        case 32:
          return OpCodes.LoadAndCompareQQWordB(source, comparison, 0);
        case 64:
          return OpCodes.LoadAndCompareDQQWordB(source, comparison, 0);
        default:

          // Compare in 64-byte chunks
          for (; count >= 64; source += 64, comparison += 64, count -= 64)
            if (!OpCodes.LoadAndCompareDQQWordB(source, comparison, 0))
                return false;
            
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