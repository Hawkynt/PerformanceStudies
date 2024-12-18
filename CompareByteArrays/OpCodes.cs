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
  public static byte LoadByte(byte* mem) => *mem; 
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte LoadByte(byte* mem, int offset) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadByte(byte* mem) => *mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadByte(byte* mem, int offset) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static word LoadWord(byte* mem) => *(word*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static word LoadWord(byte* mem, int offset) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadWord(byte* mem) => *(word*)mem;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword DWLoadWord(byte* mem, int offset) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword LoadDWord(byte* mem) => *(dword*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dword LoadDWord(byte* mem, int offset) => *(dword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadByte(byte* mem) => *mem;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadByte(byte* mem, int offset) => mem[offset];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadWord(byte* mem) => *(word*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadWord(byte* mem, int offset) => *(word*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadDWord(byte* mem) => *(dword*)mem; 
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword QWLoadDWord(byte* mem, int offset) => *(dword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadQWord(byte* mem) => *(qword*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qword LoadQWord(byte* mem, int offset) => *(qword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword LoadDQWord(byte* mem) => *(dqword*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqword LoadDQWord(byte* mem, int offset) => *(dqword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword LoadQQWord(byte* mem) => *(qqword*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static qqword LoadQQWord(byte* mem, int offset) => *(qqword*)(mem + offset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword LoadDQQWord(byte* mem) => *(dqqword*)mem;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static dqqword LoadDQQWord(byte* mem, int offset) => *(dqqword*)(mem + offset);

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
  public static qword LoadAndCompareDQWord(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return LoadDQWord(memory, offset).Equals(LoadDQWord(comparand, offset)) ? 1 : 0;

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);

    Compare(ref v0, c0);
    Compare(ref v1, c1);

    return Or(v0, v1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareDQWordB(byte* memory, byte* comparand, int offset) {

    // prefer SSE
    if (RuntimeConfiguration.IsVector128HardwareAccelerated)
      return LoadDQWord(memory, offset).Equals(LoadDQWord(comparand, offset));

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);

    Compare(ref v0, c0);
    Compare(ref v1, c1);

    return IsZero(v0, v1);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool LoadAndCompareQQWordB(byte* memory, byte* comparand, int offset) {

    // prefer AVX2
    if (RuntimeConfiguration.IsVector256HardwareAccelerated)
      return LoadQQWord(memory, offset).Equals(LoadQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector128HardwareAccelerated) {
      var x0 = LoadDQWord(memory, offset);
      var x1 = LoadDQWord(memory, offset + 16);
      var cx0 = LoadDQWord(comparand, offset);
      var cx1 = LoadDQWord(comparand, offset + 16);
      x0 = Xor(x0, cx0);
      x1 = Xor(x1, cx1);
      x0 = Or(x0, x1);
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
      return LoadDQQWord(memory, offset).Equals(LoadDQQWord(comparand, offset));

    if (RuntimeConfiguration.IsVector256HardwareAccelerated) {
      var y0 = LoadQQWord(memory, offset);
      var y1 = LoadQQWord(memory, offset + 32);
      var cy0 = LoadQQWord(comparand, offset);
      var cy1 = LoadQQWord(comparand, offset + 32);
      y0 = Xor(y0, cy0);
      y1 = Xor(y1, cy1);
      y0 = Or(y0, y1);
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
      x0 = Xor(x0, cx0);
      x1 = Xor(x1, cx1);
      x2 = Xor(x2, cx2);
      x3 = Xor(x3, cx3);
      x0 = Or(x0, x1);
      x2 = Or(x2, x3);
      return IsZero(Or(x0, x2));
    }

    var v0 = LoadQWord(memory, offset);
    var v1 = LoadQWord(memory, offset + 8);
    var c0 = LoadQWord(comparand, offset);
    var c1 = LoadQWord(comparand, offset + 8);
    Compare(ref v0, c0);
    var v2 = LoadQWord(memory, offset + 16);
    var c2 = LoadQWord(comparand, offset + 16);
    Compare(ref v1, c1);
    var v3 = LoadQWord(memory, offset + 24);
    var c3 = LoadQWord(comparand, offset + 24);
    Compare(ref v2, c2);
    v0 = Or(v0, v1);

    var v4 = LoadQWord(memory, offset + 32);
    var c4 = LoadQWord(comparand, offset + 32);
    Compare(ref v3, c3);
    v0 = Or(v0, v2);

    var v5 = LoadQWord(memory, offset + 40);
    var c5 = LoadQWord(comparand, offset + 40);
    Compare(ref v4, c4);
    v0 = Or(v0, v3);

    var v6 = LoadQWord(memory, offset + 48);
    var c6 = LoadQWord(comparand, offset + 48);
    Compare(ref v5, c5);
    v0 = Or(v0, v4);

    var v7 = LoadQWord(memory, offset + 56);
    var c7 = LoadQWord(comparand, offset + 56);
    Compare(ref v6, c6);
    v0 = Or(v0, v5);
    Compare(ref v7, c7);
    v0 = Or(v0, v6);
    v0 = Or(v0, v7);

    return IsZero(v0);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(byte al) => al == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(word ax) => ax == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(int eax) => eax == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword rax) => rax == 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword xmm) => IsEqual(xmm, dqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qqword ymm) => IsEqual(ymm, qqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqqword zmm) => IsEqual(zmm, dqqword.Zero);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword ax, dword bx) => IsZero(Or(ax, bx));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword ax, dword bx, dword cx) => IsZero(Or(ax, bx), cx);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword ax, dword bx, dword cx, dword dx) => IsZero(Or(ax, bx), Or(cx, dx));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword ax, dword bx, dword cx, dword dx,dword es) => IsZero(Or(ax, bx), Or(cx, dx), es);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dword ax, dword bx, dword cx, dword dx, dword es, dword si, dword ds, dword di) => IsZero(Or(ax, bx), Or(cx, dx), Or(es, si), Or(ds, di));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword rax, qword rbx) => IsZero(Or(rax, rbx));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword xmm0, dqword xmm1) => IsZero(Or(xmm0, xmm1));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword rax, qword rbx, qword rcx) => IsZero(Or(rax, rcx), rbx);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(dqword xmm0, dqword xmm1, dqword xmm2) => IsZero(Or(xmm0, xmm1), xmm2);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword rax, qword rbx, qword rcx, qword rdx) => IsZero(Or(rax, rbx), Or(rcx, rdx));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsZero(qword rax, qword rbx, qword rcx, qword rdx, qword res, qword rsi, qword rds, qword rdi) => IsZero(Or(rax, rbx), Or(rcx, rdx), Or(res, rsi), Or(rds, rdi));
  
}
