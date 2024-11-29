using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Classes;

namespace PerformanceStudies;

public class Program {
  public static void Main(string[] args) => BenchmarkRunner.Run<Benchmark>();
}

[MemoryDiagnoser]
public class Benchmark {

  // Define the different lengths to test
  [Params(
    0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
    10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
    20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
    30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
    40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
    50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
    60, 61, 62, 63, 64,
    1<<7, 1 << 8, 1 << 9, 1 << 10, 1 << 11, 1 << 12, 1 << 13, 1 << 14, 1 << 15, 1 << 16, 1 << 20, 1 << 24, 1 << 30
  )]
  public int Length { get; set; }

  private byte[] source;
  private byte[] comparison;

  [GlobalSetup]
  public void Setup() {
    source = new byte[Length];
    comparison = new byte[Length];
    new Random(42).NextBytes(source);
    Array.Copy(source, comparison, Length);
  }

  [Benchmark]
  public bool CompareEqualArraysSpanSequenceEquals() => BlockComparerSpanSequenceEquals.IsEqual(source, Length, comparison, Length);

  [Benchmark(Baseline = true)]
  public bool CompareEqualArraysNaïve() => BlockComparerNaïve.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysBoundsFree() => BlockComparerBoundsFree.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysUnrolled() => BlockComparerUnrolled.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysBranchMinimized() => BlockComparerBranchMinimized.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysNoDataDependency() => BlockComparerInstructionLevelParallelism.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysChunks() => BlockComparerChunks.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysChunksUnrolled() => BlockComparerChunksUnrolled.IsEqual(source, Length, comparison, Length);
  
  [Benchmark]
  public bool CompareEqualArraysJumpTable() => BlockComparerJumpTable.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysVector() => BlockComparerVector.IsEqual(source, Length, comparison, Length);

  [Benchmark]
  public bool CompareEqualArraysInterleaved() => BlockComparerInterleaved.IsEqual(source, Length, comparison, Length);

}