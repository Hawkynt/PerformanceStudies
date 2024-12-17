namespace System.Runtime.Intrinsics;
using InteropServices;

#if !SUPPORTS_VECTOR_128
internal struct Vector128 {
  public static bool IsHardwareAccelerated => false;
}

[StructLayout(LayoutKind.Sequential, Size = 16)]
internal struct Vector128<T> where T : struct {
  public static readonly Vector128<T> Zero = new();
}
#endif

#if !SUPPORTS_VECTOR_256
internal struct Vector256 {
  public static bool IsHardwareAccelerated => false;
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
internal struct Vector256<T> where T : struct {
  public static readonly Vector256<T> Zero = new();
}
#endif

#if !SUPPORTS_VECTOR_512
internal struct Vector512 {
  public static bool IsHardwareAccelerated => false;
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
internal struct Vector512<T> where T : struct {
  public static readonly Vector512<T> Zero = new();
}
#endif
