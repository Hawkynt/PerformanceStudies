using System.Runtime.CompilerServices;
using CompareByteArrays;
using word=System.Int16;
using dword = System.Int32;
using qword = System.Int64;
using dqword = System.Runtime.Intrinsics.Vector128<byte>;
using qqword = System.Runtime.Intrinsics.Vector256<byte>;
using dqqword = System.Runtime.Intrinsics.Vector512<byte>;

namespace Classes;

internal static unsafe class OpCodes {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadByte(byte* memory, int offset = 0) => memory[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadWord(byte* memory, int offset = 0) => ((word*)memory)[offset / 2];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadDWord(byte* memory, int offset = 0) => ((dword*)memory)[offset / 4];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadQWord(byte* memory, int offset = 0) => ((qword*)memory)[offset / 8];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword LoadDQWord(byte* memory, int offset = 0) => ((dqword*)memory)[offset / 16];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword LoadQQWord(byte* memory, int offset = 0) => ((qqword*)memory)[offset / 32];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword LoadDQQWord(byte* memory, int offset = 0) => ((dqqword*)memory)[offset / 64];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CompareDWord(ref dword v0, dword v1) => v0 ^= v1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CompareQWord(ref qword v0, qword v1) => v0 ^= v1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CompareDQWord(ref dqword v0, dqword v1) => v0 ^= v1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadAndCompareDQWord(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return LoadDQWord(memory, offset).Equals(LoadDQWord(comparand, offset))?1:0;

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);

    CompareQWord(ref v0, c0);
    CompareQWord(ref v1, c1);

    return v0 | v1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareDQWordB(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return LoadDQWord(memory,offset).Equals(LoadDQWord(comparand, offset));

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);

    CompareQWord(ref v0, c0);
    CompareQWord(ref v1, c1);

    return IsZero(v0, v1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareQQWordB(byte* memory, byte* comparand, int offset) {

    // prefer AVX2
    if (RuntimeConfiguration.IsVector256HardwareAccelerated)
      return LoadQQWord(memory, offset).Equals(LoadQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector128HardwareAccelerated) {
      var x0= LoadDQWord(memory, offset);
      var x1 = LoadDQWord(memory, offset+16);
      var cx0 = LoadDQWord(comparand, offset);
      var cx1 = LoadDQWord(comparand, offset + 16);
      x0 ^= cx0;
      x1 ^= cx1;
      x0 |= x1;
      return IsZero(x0);
    }

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

    return IsZero(v0, v1, v2, v3);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareDQQWordB(byte* memory, byte* comparand, int offset) {

    // prefer AVX512
    if (RuntimeConfiguration.IsVector512HardwareAccelerated)
      return LoadDQQWord(memory, offset).Equals(LoadDQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector256HardwareAccelerated) {
      var y0 = LoadQQWord(memory, offset);
      var y1 = LoadQQWord(memory, offset + 32);
      var cy0 = LoadQQWord(comparand, offset);
      var cy1 = LoadQQWord(comparand, offset + 32);
      y0 ^= cy0;
      y1 ^= cy1;
      y0 |= y1;
      return IsZero(y0);
    }

    if (RuntimeConfiguration.IsVector128HardwareAccelerated) {
      var x0 = LoadQQWord(memory, offset);
      var x1 = LoadQQWord(memory, offset + 16);
      var x2 = LoadQQWord(memory, offset + 32);
      var x3 = LoadQQWord(memory, offset + 48);
      var cx0 = LoadQQWord(comparand, offset);
      var cx1 = LoadQQWord(comparand, offset + 16);
      var cx2 = LoadQQWord(comparand, offset + 32);
      var cx3 = LoadQQWord(comparand, offset + 48);
      x0 ^= cx0;
      x1 ^= cx1;
      x2 ^= cx2;
      x3 ^= cx3;
      x0 |= x1;
      x2 |= x3;
      return IsZero(x0 | x2);
    }

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);
    CompareQWord(ref v0, c0);
    var v2 = LoadQWord(memory, offset + 16);
    var c2 = LoadQWord(comparand, offset + 16);
    CompareQWord(ref v1, c1);
    var v3 = LoadQWord(memory, offset + 24);
    var c3 = LoadQWord(comparand, offset + 24);
    CompareQWord(ref v2, c2);
    v0 |= v1;
    
    var v4 = LoadQWord(memory, offset + 32);
    var c4 = LoadQWord(comparand, offset + 32);
    CompareQWord(ref v3, c3);
    v0 |= v2;

    var v5 = LoadQWord(memory, offset + 40);
    var c5 = LoadQWord(comparand, offset + 40);
    CompareQWord(ref v4, c4);
    v0 |= v3;

    var v6 = LoadQWord(memory, offset + 48);
    var c6 = LoadQWord(comparand, offset + 48);
    CompareQWord(ref v5, c5);
    v0 |= v4;

    var v7 = LoadQWord(memory, offset + 56);
    var c7 = LoadQWord(comparand, offset + 56);
    CompareQWord(ref v6, c6);
    v0 |= v5;
    CompareQWord(ref v7, c7);
    v0 |= v6;
    v0 |= v7;
    
    return IsZero(v0);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(byte v0) => v0 == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(word v0) => v0 == 0; 
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(int v0) => v0 == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0) => v0 == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword v0) => v0 == dqword.Zero;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qqword v0) => v0 == qqword.Zero;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqqword v0) => v0 == dqqword.Zero;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1) => (v0 | v1) == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1, dword v2) => IsZero(v0 | v2, v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1, dword v2, dword v3) => IsZero(v0 | v2, v1 | v3);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1) => (v0 | v1) == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword v0, dqword v1) => (v0 | v1) == dqword.Zero;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2) => IsZero(v0 | v2, v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword v0, dqword v1, dqword v2) => IsZero(v0 | v2, v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2, qword v3) => IsZero(v0 | v2, v1 | v3);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2, qword v3, qword v4, qword v5, qword v6, qword v7) => IsZero(v0 | v4, v1 | v5, v2 | v6, v3 | v7);

}
