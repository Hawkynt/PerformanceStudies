using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using CompareByteArrays;

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
        int r0 = source[0];
        int r1 = source[1];
        int r2 = source[2];
        r0 ^= comparison[0];
        r1 ^= comparison[1];
        r2 ^= comparison[2];
        r0 |= r1;
        r0 |= r2;
        return r0 == 0;
      }
      case 4: {
        int r0 = source[0];
        int r1 = source[1];
        int r2 = source[2];
        int r3 = source[3];
        r0 ^= comparison[0];
        r1 ^= comparison[1];
        r2 ^= comparison[2];
        r3 ^= comparison[3];
        r0 |= r1;
        r2 |= r3;
        r0 |= r2;
        return r0 == 0;
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
    int e0, e1, e2,e3;
    long r0, r1, r2, r3, r4, r5, r6, r7, r8;
    for (;;)
      switch (count) {
        case 0: return true;
        case 1: return *source == *comparison;
        case 2: return *(short*)source == *(short*)comparison;
        case 3: {
          e0 = *(short*)source;
          e1 = source[2];
          e2 = *(short*)comparison;
          e3 = comparison[2];
          e0 ^= e2;
          e1 ^= e3;
          e0 |= e1;
          return e0 != 0;
        }
        case 4: return *(int*)source == *(int*)comparison;
        case 5:
          e0 = *(int*)source;
          e1 = source[4];
          e2 = *(int*)comparison;
          e3 = comparison[4];
          e0 ^= e2;
          e1 ^= e3;
          e0 |= e1;
          return e0 != 0;
        case 6:
          e0 = *(int*)source;
          e1 = ((short*)source)[2];
          e0 ^= *(int*)comparison;
          e1 ^= ((short*)comparison)[2];
          e0 |= e1;
          return e0 != 0;
        case 7:
          e0 = *(int*)source;
          e1 = ((short*)source)[2];
          e2 = source[6];
          e0 ^= *(int*)comparison;
          e1 ^= ((short*)comparison)[2];
          e2 ^= comparison[6];
          e0 |= e1;
          e0 |= e2;
          return e0 != 0;
        case 8 when RuntimeConfiguration.IsLongHardwareAccelerated: 
          return *(long*)source == *(long*)comparison;
        case 8:
          e0 = *(int*)source;
          e1 = *(int*)(source + 8);
          e2 = *(int*)comparison;
          e3 = *(int*)(comparison + 8);
          e0 ^= e2;
          e1 ^= e3;
          e0 |= e1;
          return e0 != 0;
        case 9:
          r0 = LoadQWord(source, 0);
          r1 = LoadByte(source, 8);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadByte(comparison, 8);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          return IsZero2(r0, r1);
        case 10:
          r0 = LoadQWord(source, 0);
          r1 = LoadWord(source, 8);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadWord(comparison, 8);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          return IsZero2(r0, r1);
        case 11:
          r0 = LoadQWord(source, 0);
          r1 = LoadWord(source, 8);
          r2 = LoadByte(source, 10);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadWord(comparison, 8);
          r6 = LoadByte(comparison, 10);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          CompareQWord(ref r2, r6);
          return IsZero2(r0 | r2, r1);
        case 12:
          r0 = LoadQWord(source, 0);
          r1 = LoadDWord(source, 8);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadDWord(comparison, 8);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          return IsZero2(r0, r1);
        case 13:
          r0 = LoadQWord(source, 0);
          r1 = LoadDWord(source, 8);
          r2 = LoadByte(source, 12);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadDWord(comparison, 8);
          r6 = LoadByte(comparison, 12);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          CompareQWord(ref r2, r6);
          return IsZero2(r0 | r2, r1);
        case 14:
          r0 = LoadQWord(source, 0);
          r1 = LoadDWord(source, 8);
          r2 = LoadWord(source, 12);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadDWord(comparison, 8);
          r6 = LoadWord(comparison, 12);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          CompareQWord(ref r2, r6);
          return IsZero2(r0 | r2, r1);
        case 15:
          r0 = LoadQWord(source, 0);
          r1 = LoadDWord(source, 8);
          r2 = LoadWord(source, 12);
          r3 = LoadByte(source, 14);
          r4 = LoadQWord(comparison, 0);
          r5 = LoadDWord(comparison, 8);
          r6 = LoadWord(comparison, 12);
          r7 = LoadByte(comparison, 14);
          CompareQWord(ref r0, r4);
          CompareQWord(ref r1, r5);
          CompareQWord(ref r2, r6);
          CompareQWord(ref r3, r7);
          return IsZero4(r0, r1, r2, r3);
        case 16:
          return LoadAndCompareDQWordB(source, comparison, 0);
        case 17:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadByte(source, 16);
          r4 = LoadByte(comparison, 16);
          CompareQWord(ref r1, r4);
          return IsZero2(r0, r1);
        case 18:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadWord(source, 16);
          r4 = LoadWord(comparison, 16);
          CompareQWord(ref r1, r4);
          return IsZero2(r0, r1);
        case 19:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadWord(source, 16);
          r2 = LoadByte(source, 18);
          r4 = LoadWord(comparison, 16);
          r5 = LoadByte(comparison, 18);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 20:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadDWord(source, 16);
          r4 = LoadDWord(comparison, 16);
          CompareQWord(ref r1, r4);
          return IsZero2(r0, r1);
        case 21:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadDWord(source, 16);
          r2 = LoadByte(source, 20);
          r4 = LoadDWord(comparison, 16);
          r5 = LoadByte(comparison, 20);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 22:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadDWord(source, 16);
          r2 = LoadWord(source, 20);
          r4 = LoadDWord(comparison, 16);
          r5 = LoadWord(comparison, 20);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 23:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadDWord(source, 16);
          r2 = LoadWord(source, 20);
          r3 = LoadByte(source, 22);
          r4 = LoadDWord(comparison, 16);
          r5 = LoadWord(comparison, 20);
          r6 = LoadByte(comparison, 22);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          CompareQWord(ref r3, r6);
          return IsZero4(r0, r1, r2, r3);
        case 24:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r4 = LoadQWord(comparison, 16);
          CompareQWord(ref r1, r4);
          return IsZero2(r0, r1);
        case 25:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadByte(source, 24);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadByte(comparison, 24);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 26:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadWord(source, 24);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadWord(comparison, 24);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 27:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadWord(source, 24);
          r3 = LoadByte(source, 26);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadWord(comparison, 24);
          r6 = LoadByte(comparison, 26);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          CompareQWord(ref r3, r6);
          return IsZero4(r0, r1, r2, r3);
        case 28:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadDWord(source, 24);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadDWord(comparison, 24);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          return IsZero2(r0 | r2, r1);
        case 29:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadDWord(source, 24);
          r3 = LoadByte(source, 28);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadDWord(comparison, 24);
          r6 = LoadByte(comparison, 28);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          CompareQWord(ref r3, r6);
          return IsZero4(r0, r1, r2, r3);
        case 30:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadDWord(source, 24);
          r3 = LoadWord(source, 28);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadDWord(comparison, 24);
          r6 = LoadWord(comparison, 28);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          CompareQWord(ref r3, r6);
          return IsZero4(r0, r1, r2, r3);
        case 31:
          r0 = LoadAndCompareDQWord(source, comparison, 0);
          r1 = LoadQWord(source, 16);
          r2 = LoadDWord(source, 24);
          r3 = LoadWord(source, 28);
          r8 = LoadByte(source, 30);
          r4 = LoadQWord(comparison, 16);
          r5 = LoadDWord(comparison, 24);
          r6 = LoadWord(comparison, 28);
          r7 = LoadByte(comparison, 30);
          CompareQWord(ref r1, r4);
          CompareQWord(ref r2, r5);
          CompareQWord(ref r3, r6);
          CompareQWord(ref r8, r7);
          return IsZero4(r0 | r8, r1, r2, r3);
        case 32:
          return LoadAndCompareQQWordB(source, comparison, 0);
        case 64:
          return LoadAndCompareDQQWordB(source, comparison, 0);
        default:

          // Compare in 64-byte chunks
          for (; count >= 64; source += 64, comparison += 64, count -= 64)
            if (!LoadAndCompareDQQWordB(source, comparison, 0))
                return false;
            
          // Compare a 32-byte chunk
          if (count >= 32) {
            if (!LoadAndCompareQQWordB(source, comparison, 0))
              return false;

            source += 32;
            comparison += 32;
            count -= 32;
          }
          
          // less than 32 bytes, use the "perfect" case
          break;
      }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long LoadByte(byte* memory, int offset) => memory[offset];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long LoadWord(byte* memory, int offset) => ((short*)memory)[offset / 2];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long LoadDWord(byte* memory, int offset) => ((int*)memory)[offset / 4];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long LoadQWord(byte* memory, int offset) => ((long*)memory)[offset / 8];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void CompareQWord(ref long v0, long v1) => v0 ^= v1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool LoadAndCompareDQWordB(byte* memory, byte* comparand, int offset) {
      
      // prefer SSE
      if (Vector128.IsHardwareAccelerated)
        return Vector128.Load(source + offset).Equals(Vector128.Load(comparison + offset));

      var v0 = LoadQWord(memory, offset);
      var v1 = LoadQWord(memory, offset + 8);
      var c0 = LoadQWord(comparand, offset);
      var c1 = LoadQWord(comparand, offset + 8);

      CompareQWord(ref v0, c0);
      CompareQWord(ref v1, c1);

      return IsZero2(v0, v1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool LoadAndCompareQQWordB(byte* memory, byte* comparand, int offset) {
      
      // prefer AVX2
      if (Vector256.IsHardwareAccelerated)
        return Vector256.Load(source + offset).Equals(Vector256.Load(comparison + offset));

      if(Vector128.IsHardwareAccelerated)
        return 
          Vector128.Load(source + offset).Equals(Vector128.Load(comparison + offset))
          & Vector128.Load(source + offset + 16).Equals(Vector128.Load(comparison + offset + 16));

      var v0 = LoadQWord(memory, offset);
      var v1 = LoadQWord(memory, offset + 8);
      var v2 = LoadQWord(memory, offset + 16);
      var v3 = LoadQWord(memory, offset + 24);
      var c0 = LoadQWord(comparand, offset);
      var c1 = LoadQWord(comparand, offset + 8);
      var c2 = LoadQWord(comparand, offset + 16);
      var c3 = LoadQWord(comparand, offset + 24);

      CompareQWord(ref v0, c0);
      CompareQWord(ref v1, c1);
      CompareQWord(ref v2, c2);
      CompareQWord(ref v3, c3);

      return IsZero4(v0, v1, v2, v3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool LoadAndCompareDQQWordB(byte* memory, byte* comparand, int offset) {
      
      // prefer AVX512
      if (Vector512.IsHardwareAccelerated)
        return Vector512.Load(source + offset).Equals(Vector512.Load(comparison + offset));

      if (Vector256.IsHardwareAccelerated)
        return Vector256.Load(source + offset).Equals(Vector256.Load(comparison + offset))
          & Vector256.Load(source + offset + 32).Equals(Vector256.Load(comparison + offset + 32));

      if (Vector128.IsHardwareAccelerated)
        return
          Vector128.Load(source + offset).Equals(Vector128.Load(comparison + offset))
          & Vector128.Load(source + offset + 16).Equals(Vector128.Load(comparison + offset + 16))
          & Vector128.Load(source + offset + 32).Equals(Vector128.Load(comparison + offset + 32))
          & Vector128.Load(source + offset + 48).Equals(Vector128.Load(comparison + offset + 48));

      var v0 = LoadQWord(memory, offset);
      var v1 = LoadQWord(memory, offset + 8);
      var v2 = LoadQWord(memory, offset + 16);
      var v3 = LoadQWord(memory, offset + 24);
      var v4 = LoadQWord(memory, offset + 32);
      var v5 = LoadQWord(memory, offset + 40);
      var v6 = LoadQWord(memory, offset + 48);
      var v7 = LoadQWord(memory, offset + 56);
      var c0 = LoadQWord(comparand, offset);
      var c1 = LoadQWord(comparand, offset + 8);
      var c2 = LoadQWord(comparand, offset + 16);
      var c3 = LoadQWord(comparand, offset + 24);
      var c4 = LoadQWord(comparand, offset + 32);
      var c5 = LoadQWord(comparand, offset + 40);
      var c6 = LoadQWord(comparand, offset + 48);
      var c7 = LoadQWord(comparand, offset + 56);

      CompareQWord(ref v0, c0);
      CompareQWord(ref v1, c1);
      CompareQWord(ref v2, c2);
      CompareQWord(ref v3, c3);
      CompareQWord(ref v4, c4);
      CompareQWord(ref v5, c5);
      CompareQWord(ref v6, c6);
      CompareQWord(ref v7, c7);

      return IsZero8(v0, v1, v2, v3, v4, v5, v6, v7);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long LoadAndCompareDQWord(byte* memory, byte* comparand, int offset) {
      if (Vector128.IsHardwareAccelerated)
        return Vector128.Load(source + offset).Equals(Vector128.Load(comparison + offset)) ? 0 : 1;

      var v0 = LoadQWord(memory, offset);
      var v1 = LoadQWord(memory, offset + 8);
      var c0 = LoadQWord(comparand, offset);
      var c1 = LoadQWord(comparand, offset + 8);

      CompareQWord(ref v0, c0);
      CompareQWord(ref v1, c1);

      return v0 | v1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsZero2(long v0, long v1) => (v0 | v1) == 0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsZero4(long v0, long v1, long v2, long v3) => IsZero2(v0 | v2, v1 | v3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsZero8(long v0, long v1, long v2, long v3, long v4, long v5, long v6, long v7) => IsZero4(v0 | v4, v1 | v5, v2 | v6, v3 | v7);

  }

}