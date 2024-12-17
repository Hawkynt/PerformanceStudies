#define SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED
namespace Corlib;

#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED
using System.Runtime.Intrinsics;
#endif
using System;

internal static class RuntimeConfiguration {

  public static bool IsLongHardwareAccelerated => IntPtr.Size == 8;
#if SUPPORTS_VECTOR_IS_HARDWARE_ACCELERATED
  public static bool IsVector128HardwareAccelerated => Vector128.IsHardwareAccelerated;
  public static bool IsVector256HardwareAccelerated => Vector256.IsHardwareAccelerated;
  public static bool IsVector512HardwareAccelerated => Vector512.IsHardwareAccelerated;
#else
  public static bool IsVector128HardwareAccelerated => false;
  public static bool IsVector256HardwareAccelerated => false;
  public static bool IsVector512HardwareAccelerated => false;
#endif

  // When more than 6 registers are needed, the compiler starts using `xmm6` and `xmm7`.
  // These registers must be saved to and restored from the stack for each function call
  // and return, adding overhead and slowing down execution.
  // Note: `ymm` and `zmm` registers share their lower parts with `xmm` registers, so they
  // are also impacted by this behavior.
  // If this flag is set to `false`, processing larger data chunks may be slower due to
  // increased loop overhead, but avoids the register save/restore penalty for smaller chunks.
  public static bool AllowUsingMoreThan6ExtendedRegisters => false;

  // When more than 6 general-purpose registers are needed, the compiler starts using `rsi`
  // and `rbx`. These registers are callee-saved, meaning they must be pushed onto the stack
  // at the start of a function and restored from the stack before returning. This adds
  // overhead and may slow down execution. If this flag is set to `false`, fewer registers
  // are available for processing, potentially increasing loop overhead due to additional
  // memory access, but avoids the push/pop penalty for smaller chunks.
  public static bool AllowUsingMoreThan6NormalRegisters => false;

}
