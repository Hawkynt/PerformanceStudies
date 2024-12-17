using System.Runtime.CompilerServices;
using word=System.Int16;
using dword = System.Int32;
using qword = System.Int64;
using dqword = System.Runtime.Intrinsics.Vector128<byte>;
using qqword = System.Runtime.Intrinsics.Vector256<byte>;
using dqqword = System.Runtime.Intrinsics.Vector512<byte>;

namespace Corlib;

internal static unsafe partial class OpCodes {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, byte value, int offset = 0) => mem[offset] = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, word value, int offset = 0) => *(word*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, dword value, int offset = 0) => *(dword*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, qword value, int offset = 0) => *(qword*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, dqword value, int offset = 0) => *(dqword*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, qqword value, int offset = 0) => *(qqword*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Store(byte* mem, dqqword value, int offset = 0) => *(dqqword*)(mem + offset) = value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte LoadByte(byte* mem, int offset = 0) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadByte(byte* mem, int offset = 0) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static word WLoadWord(byte* mem, int offset = 0) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadWord(byte* mem, int offset = 0) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadDWord(byte* mem, int offset = 0) => *(dword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadByte(byte* mem, int offset = 0) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadWord(byte* mem, int offset = 0) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadDWord(byte* mem, int offset = 0) => *(dword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadQWord(byte* mem, int offset = 0) => *(qword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword DQWLoadDQWord(byte* mem, int offset = 0) => *(dqword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword QQWLoadQQWord(byte* mem, int offset = 0) => *(qqword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword DQQWLoadDQQWord(byte* mem, int offset = 0) => *(dqqword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword Compare(ref dword accumulator, dword comparand) => accumulator = Xor(accumulator, comparand);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword Compare(ref qword accumulator, qword comparand) => accumulator = Xor(accumulator, comparand);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword Compare(ref dqword accumulator, dqword comparand) => accumulator = Xor(accumulator, comparand);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword Compare(ref qqword accumulator, qqword comparand) => accumulator = Xor(accumulator, comparand);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword Compare(ref dqqword accumulator, dqqword comparand) => accumulator = Xor(accumulator, comparand);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(byte b0, byte b1) => b0 == b1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(word w0, word w1) => w0 == w1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(dword e0, dword e1) => e0 == e1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(qword r0, qword r1) => r0 == r1;

#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(dqword x0, dqword x1) => x0 == x1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(qqword y0, qqword y1) => y0 == y1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEqual(dqqword z0, dqqword z1) => z0 == z1;

#else

  public static bool IsEqual(dqword x0, dqword x1) {
    var r0 = (qword*)&x0;
    var r1 = (qword*)&x1;
    for (var i = 0; i < 2; ++i)
      if (r0[i] != r1[i])
        return false;

    return true;
  }

  public static bool IsEqual(qqword y0, qqword y1) {
    var r0 = (qword*)&y0;
    var r1 = (qword*)&y1;
    for (var i = 0; i < 4; ++i)
      if (r0[i] != r1[i])
        return false;

    return true;
  }

  public static bool IsEqual(dqqword z0, dqqword z1) {
    var r0 = (qword*)&z0;
    var r1 = (qword*)&z1;
    for (var i = 0; i < 8; ++i)
      if (r0[i] != r1[i])
        return false;

    return true;
  }

#endif

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadAndCompareDQWord(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return DQWLoadDQWord(memory, offset).Equals(DQWLoadDQWord(comparand, offset)) ? 1 : 0;

    var v0 = QWLoadQWord(memory, offset);
    var v1 = QWLoadQWord(memory, offset + 8);
    var c0 = QWLoadQWord(comparand, offset);
    var c1 = QWLoadQWord(comparand, offset + 8);

    Compare(ref v0, c0);
    Compare(ref v1, c1);

    return Or(v0, v1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareDQWordB(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return DQWLoadDQWord(memory, offset).Equals(DQWLoadDQWord(comparand, offset));

    var v0 = QWLoadQWord(memory, offset);
    var v1 = QWLoadQWord(memory, offset + 8);
    var c0 = QWLoadQWord(comparand, offset);
    var c1 = QWLoadQWord(comparand, offset + 8);

    Compare(ref v0, c0);
    Compare(ref v1, c1);

    return IsZero(v0, v1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareQQWordB(byte* memory, byte* comparand, int offset) {

    // prefer AVX2
    if (RuntimeConfiguration.IsVector256HardwareAccelerated)
      return QQWLoadQQWord(memory, offset).Equals(QQWLoadQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector128HardwareAccelerated) {
      var x0 = DQWLoadDQWord(memory, offset);
      var x1 = DQWLoadDQWord(memory, offset + 16);
      var cx0 = DQWLoadDQWord(comparand, offset);
      var cx1 = DQWLoadDQWord(comparand, offset + 16);
      x0 = Xor(x0, cx0);
      x1 = Xor(x1, cx1);
      x0 = Or(x0, x1);
      return IsZero(x0);
    }

    var v0 = QWLoadQWord(memory, offset);
    var v1 = QWLoadQWord(memory, offset + 8);
    var v2 = QWLoadQWord(memory, offset + 16);
    var v3 = QWLoadQWord(memory, offset + 24);
    var c0 = QWLoadQWord(comparand, offset);
    var c1 = QWLoadQWord(comparand, offset + 8);
    var c2 = QWLoadQWord(comparand, offset + 16);
    var c3 = QWLoadQWord(comparand, offset + 24);

    Compare(ref v0, c0);
    Compare(ref v1, c1);
    Compare(ref v2, c2);
    Compare(ref v3, c3);

    return IsZero(v0, v1, v2, v3);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareDQQWordB(byte* memory, byte* comparand, int offset) {

    // prefer AVX512
    if (RuntimeConfiguration.IsVector512HardwareAccelerated)
      return DQQWLoadDQQWord(memory, offset).Equals(DQQWLoadDQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector256HardwareAccelerated) {
      var y0 = QQWLoadQQWord(memory, offset);
      var y1 = QQWLoadQQWord(memory, offset + 32);
      var cy0 = QQWLoadQQWord(comparand, offset);
      var cy1 = QQWLoadQQWord(comparand, offset + 32);
      y0 = Xor(y0, cy0);
      y1 = Xor(y1, cy1);
      y0 = Or(y0, y1);
      return IsZero(y0);
    }

    if (RuntimeConfiguration.IsVector128HardwareAccelerated) {
      var x0 = QQWLoadQQWord(memory, offset);
      var x1 = QQWLoadQQWord(memory, offset + 16);
      var x2 = QQWLoadQQWord(memory, offset + 32);
      var x3 = QQWLoadQQWord(memory, offset + 48);
      var cx0 = QQWLoadQQWord(comparand, offset);
      var cx1 = QQWLoadQQWord(comparand, offset + 16);
      var cx2 = QQWLoadQQWord(comparand, offset + 32);
      var cx3 = QQWLoadQQWord(comparand, offset + 48);
      x0 = Xor(x0, cx0);
      x1 = Xor(x1, cx1);
      x2 = Xor(x2, cx2);
      x3 = Xor(x3, cx3);
      x0 = Or(x0, x1);
      x2 = Or(x2, x3);
      return IsZero(Or(x0, x2));
    }

    var v0 = QWLoadQWord(memory, offset);
    var v1 = QWLoadQWord(memory, offset + 8);
    var c0 = QWLoadQWord(comparand, offset);
    var c1 = QWLoadQWord(comparand, offset + 8);
    Compare(ref v0, c0);
    var v2 = QWLoadQWord(memory, offset + 16);
    var c2 = QWLoadQWord(comparand, offset + 16);
    Compare(ref v1, c1);
    var v3 = QWLoadQWord(memory, offset + 24);
    var c3 = QWLoadQWord(comparand, offset + 24);
    Compare(ref v2, c2);
    v0 = Or(v0, v1);

    var v4 = QWLoadQWord(memory, offset + 32);
    var c4 = QWLoadQWord(comparand, offset + 32);
    Compare(ref v3, c3);
    v0 = Or(v0, v2);

    var v5 = QWLoadQWord(memory, offset + 40);
    var c5 = QWLoadQWord(comparand, offset + 40);
    Compare(ref v4, c4);
    v0 = Or(v0, v3);

    var v6 = QWLoadQWord(memory, offset + 48);
    var c6 = QWLoadQWord(comparand, offset + 48);
    Compare(ref v5, c5);
    v0 = Or(v0, v4);

    var v7 = QWLoadQWord(memory, offset + 56);
    var c7 = QWLoadQWord(comparand, offset + 56);
    Compare(ref v6, c6);
    v0 = Or(v0, v5);
    Compare(ref v7, c7);
    v0 = Or(v0, v6);
    v0 = Or(v0, v7);

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
  public static bool IsZero(dqword v0) => IsEqual(v0, dqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qqword v0) => IsEqual(v0, qqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqqword v0) => IsEqual(v0, dqqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1) => IsZero(Or(v0, v1));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1, dword v2) => IsZero(Or(v0, v2), v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword v0, dword v1, dword v2, dword v3) => IsZero(Or(v0, v2), Or(v1, v3));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1) => IsZero(Or(v0, v1));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword v0, dqword v1) => IsZero(Or(v0, v1));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2) => IsZero(Or(v0, v2), v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword v0, dqword v1, dqword v2) => IsZero(Or(v0, v2), v1);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2, qword v3) => IsZero(Or(v0, v2), Or(v1, v3));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword v0, qword v1, qword v2, qword v3, qword v4, qword v5, qword v6, qword v7) => IsZero(Or(v0, v4), Or(v1, v5), Or(v2, v6), Or(v3, v7));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword And(dword e0, dword e1) => e0 & e1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword And(qword r0, qword r1) => r0 & r1;

#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword And(dqword x0, dqword x1) => x0 & x1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword And(qqword y0, qqword y1) => y0 & y1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword And(dqqword z0, dqqword z1) => z0 & z1;

#else

  public static dqword And(dqword x0, dqword x1) {
    var r0 = (qword*)&x0;
    var r1 = (qword*)&x1;
    for (var i = 0; i < 2; ++i)
      r0[i] &= r1[i];

    return x0;
  }

  public static qqword And(qqword y0, qqword y1) {
    var r0 = (qword*)&y0;
    var r1 = (qword*)&y1;
    for (var i = 0; i < 4; ++i)
      r0[i] &= r1[i];

    return y0;
  }

  public static dqqword And(dqqword z0, dqqword z1) {
    var r0 = (qword*)&z0;
    var r1 = (qword*)&z1;
    for (var i = 0; i < 8; ++i)
      r0[i] &= r1[i];

    return z0;
  }

#endif

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword Or(dword e0, dword e1) => e0 | e1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword Or(qword r0, qword r1) => r0 | r1;

#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword Or(dqword x0, dqword x1) => x0 | x1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword Or(qqword y0, qqword y1) => y0 | y1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword Or(dqqword z0, dqqword z1) => z0 | z1;

#else

  public static dqword Or(dqword x0, dqword x1) {
    var r0 = (qword*)&x0;
    var r1 = (qword*)&x1;
    for (var i = 0; i < 2; ++i)
      r0[i] |= r1[i];

    return x0;
  }

  public static qqword Or(qqword y0, qqword y1) {
    var r0 = (qword*)&y0;
    var r1 = (qword*)&y1;
    for (var i = 0; i < 4; ++i)
      r0[i] |= r1[i];

    return y0;
  }

  public static dqqword Or(dqqword z0, dqqword z1) {
    var r0 = (qword*)&z0;
    var r1 = (qword*)&z1;
    for (var i = 0; i < 8; ++i)
      r0[i] |= r1[i];

    return z0;
  }

#endif

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword Xor(dword e0, dword e1) => e0 ^ e1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword Xor(qword r0, qword r1) => r0 ^ r1;

#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword Xor(dqword x0, dqword x1) => x0 ^ x1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword Xor(qqword y0, qqword y1) => y0 ^ y1;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword Xor(dqqword z0, dqqword z1) => z0 ^ z1;

#else

  public static dqword Xor(dqword x0, dqword x1) {
    var r0 = (qword*)&x0;
    var r1 = (qword*)&x1;
    for (var i = 0; i < 2; ++i)
      r0[i] ^= r1[i];

    return x0;
  }

  public static qqword Xor(qqword y0, qqword y1) {
    var r0 = (qword*)&y0;
    var r1 = (qword*)&y1;
    for (var i = 0; i < 4; ++i)
      r0[i] ^= r1[i];

    return y0;
  }

  public static dqqword Xor(dqqword z0, dqqword z1) {
    var r0 = (qword*)&z0;
    var r1 = (qword*)&z1;
    for (var i = 0; i < 8; ++i)
      r0[i] ^= r1[i];

    return z0;
  }

#endif

}
