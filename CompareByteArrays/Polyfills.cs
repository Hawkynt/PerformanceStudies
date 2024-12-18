namespace System.Runtime.Intrinsics {
  using InteropServices;

#if !SUPPORTS_VECTOR_128
  internal struct Vector128 {
    public static bool IsHardwareAccelerated => false;
  }

  [StructLayout(LayoutKind.Sequential, Size = 16)]
  internal struct Vector128<T> where T : struct {
    public static readonly Vector128<T> Zero = new();

    public static Vector128<T> operator ^ (Vector128<T> value, Vector128<T> operand) => throw new NotSupportedException();
    public static Vector128<T> operator | (Vector128<T> value, Vector128<T> operand) => throw new NotSupportedException();
    public static Vector128<T> operator & (Vector128<T> value, Vector128<T> operand) => throw new NotSupportedException();
  }
#endif

#if !SUPPORTS_VECTOR_256
  internal struct Vector256 {
    public static bool IsHardwareAccelerated => false;
  }

  [StructLayout(LayoutKind.Sequential, Size = 32)]
  internal struct Vector256<T> where T : struct {
    public static readonly Vector256<T> Zero = new();

    public static Vector256<T> operator ^ (Vector256<T> value, Vector256<T> operand) => throw new NotSupportedException();
    public static Vector256<T> operator | (Vector256<T> value, Vector256<T> operand) => throw new NotSupportedException();
    public static Vector256<T> operator & (Vector256<T> value, Vector256<T> operand) => throw new NotSupportedException();
  }
#endif

#if !SUPPORTS_VECTOR_512
  internal struct Vector512 {
    public static bool IsHardwareAccelerated => false;
  }

  [StructLayout(LayoutKind.Sequential, Size = 32)]
  internal struct Vector512<T> where T : struct {
    public static readonly Vector512<T> Zero = new();

    public static Vector512<T> operator ^ (Vector512<T> value, Vector512<T> operand) => throw new NotSupportedException();
    public static Vector512<T> operator | (Vector512<T> value, Vector512<T> operand) => throw new NotSupportedException();
    public static Vector512<T> operator & (Vector512<T> value, Vector512<T> operand) => throw new NotSupportedException();
  }
#endif

}

#if !SUPPORTS_SKIP_LOCALS_INIT

namespace System.Runtime.CompilerServices {

  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Property | AttributeTargets.Struct, Inherited = false)]
  public sealed class SkipLocalsInitAttribute : Attribute;

}

#endif