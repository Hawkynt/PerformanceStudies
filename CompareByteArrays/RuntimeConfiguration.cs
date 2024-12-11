using System.Runtime.Intrinsics;

namespace CompareByteArrays;

internal static class RuntimeConfiguration {

  public static unsafe bool IsLongHardwareAccelerated => sizeof(IntPtr) == 8;

  public static bool IsVector128HardwareAccelerated => Vector128.IsHardwareAccelerated;

  public static bool IsVector256HardwareAccelerated => Vector256.IsHardwareAccelerated;

  public static bool IsVector512HardwareAccelerated => Vector512.IsHardwareAccelerated;

}