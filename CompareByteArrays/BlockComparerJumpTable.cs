using System.Runtime.CompilerServices;

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
    int e0, e1, e2;
    long r0, r1, r2, r3, r4, r5, r6, r7;
    switch (count) {
      case 0: return true;
      case 1: return *source == *comparison;
      case 2: return *(short*)source == *(short*)comparison;
      case 3:
        e0 = *(short*)source;
        e1 = source[2];
        e0 ^= *(short*)comparison;
        e1 ^= comparison[2];
        e0 |= e1;
        return e0 != 0;
      case 4:
        e0 = *(short*)source;
        e1 = ((short*)source)[1];
        e0 ^= *(short*)comparison;
        e1 ^= ((short*)comparison)[1];
        e0 |= e1;
        return e0 != 0;
      case 5:
        e0 = *(int*)source;
        e1 = source[4];
        e0 ^= *(int*)comparison;
        e1 ^= comparison[4];
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
      case 8:
        return *(long*)source == *(long*)comparison;
      case 9:
        r0 = *(long*)source;
        r1 = source[8];
        r0 ^= *(long*)comparison;
        r1 ^= comparison[8];
        r0 |= r1;
        return r0 != 0;
      case 10:
        r0 = *(long*)source;
        r1 = ((short*)source)[4];
        r0 ^= *(long*)comparison;
        r1 ^= ((short*)comparison)[4];
        r0 |= r1;
        return r0 != 0;
      case 11:
        r0 = *(long*)source;
        r1 = ((short*)source)[4];
        r2 = source[10];
        r0 ^= *(long*)comparison;
        r1 ^= ((short*)comparison)[4];
        r2 ^= comparison[10];
        r0 |= r1;
        r0 |= r2;
        return r0 != 0;
      case 12:
        r0 = *(long*)source;
        r1 = ((int*)source)[2];
        r0 ^= *(long*)comparison;
        r1 ^= ((int*)comparison)[2];
        r0 |= r1;
        return r0 != 0;
      case 13:
        r0 = *(long*)source;
        r1 = ((int*)source)[2];
        r2 = source[12];
        r0 ^= *(long*)comparison;
        r1 ^= ((int*)comparison)[2];
        r2 ^= comparison[12];
        r0 |= r1;
        r0 |= r2;
        return r0 != 0;
      case 14:
        r0 = *(long*)source;
        r1 = ((int*)source)[2];
        r2 = ((short*)source)[6];
        r0 ^= *(long*)comparison;
        r1 ^= ((int*)comparison)[2];
        r2 ^= ((short*)comparison)[6];
        r0 |= r1;
        r0 |= r2;
        return r0 != 0;
      case 15:
        r0 = *(long*)source;
        r1 = ((int*)source)[2];
        r2 = ((short*)source)[6];
        r3 = source[14];
        r0 ^= *(long*)comparison;
        r1 ^= ((int*)comparison)[2];
        r2 ^= ((short*)comparison)[6];
        r3 ^= comparison[14];
        r0 |= r1;
        r2 |= r3;
        r0 |= r2;
        return r0 != 0;
      case 16:
        r0 = *(long*)source;
        r1 = ((long*)source)[1];
        r0 ^= *(long*)comparison;
        r1 ^= ((long*)comparison)[1];
        r0 |= r1;
        return r0 != 0;
      default:
        // Compare in 64-byte chunks using ulong
        for (; count >= 64; source += 64, comparison += 64, count -= 64) {
          // Load from source
          r0 = *(long*)source;
          r1 = ((long*)source)[1];
          r2 = ((long*)source)[2];
          r3 = ((long*)source)[3];
          r4 = ((long*)source)[4];
          r5 = ((long*)source)[5];
          r6 = ((long*)source)[6];
          r7 = ((long*)source)[7];

          // XOR with comparison
          r0 ^= *(long*)comparison;
          r1 ^= ((long*)comparison)[1];
          r2 ^= ((long*)comparison)[2];
          r3 ^= ((long*)comparison)[3];
          r4 ^= ((long*)comparison)[4];
          r5 ^= ((long*)comparison)[5];
          r6 ^= ((long*)comparison)[6];
          r7 ^= ((long*)comparison)[7];

          // Combine results using OR in stages to reduce dependency
          r0 |= r1;
          r2 |= r3;
          r4 |= r5;
          r6 |= r7;

          r0 |= r2;
          r4 |= r6;

          r0 |= r4;

          if (r0 != 0)
            return false;
        }

        // Compare in 8-byte chunks using long
        for (; count >= 8; source += 8, comparison += 8, count -= 8)
          if (*(long*)source != *(long*)comparison)
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

}