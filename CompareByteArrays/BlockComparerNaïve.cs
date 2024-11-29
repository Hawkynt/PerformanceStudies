using System.Runtime.CompilerServices;

namespace Classes;

internal static class BlockComparerNaïve {

  /// <summary>
  /// Compares two byte-arrays starting from their beginnings.
  /// </summary>
  /// <param name="source">The source.</param>
  /// <param name="sourceLength">Length of the source.</param>
  /// <param name="comparison">The comparison.</param>
  /// <param name="comparisonLength">Length of the comparison.</param>
  /// <returns><c>true</c> if both arrays contain the same data; otherwise, <c>false</c>.</returns>
  public static bool IsEqual(byte[] source, int sourceLength, byte[] comparison, int comparisonLength) {
    if (sourceLength != comparisonLength)
      return false;

    if (ReferenceEquals(source, comparison) || sourceLength <= 0)
      return true;

    return _CompareManaged(source, sourceLength, comparison);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool _CompareManaged(byte[] source, int sourceLength, byte[] comparison) {
    for (var i = 0; i < sourceLength; ++i)
      if (source[i] != comparison[i])
        return false;

    return true;
  }
}