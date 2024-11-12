# PerformanceStudies

## Overview

**PerformanceStudies** is a C# repository dedicated to demonstrating and measuring low-level performance optimizations. This collection showcases techniques aimed at maximizing CPU efficiency through microoptimizations, covering areas like cache usage, pipeline utilization, register management, and instruction execution. Each example includes benchmarks and explanations to help developers understand the impact of each optimization.

## Features

- **Cache Optimizations**: Techniques for improving memory locality and reducing cache misses.
- **Pipeline Optimizations**: Methods to reduce branch mispredictions, including loop unrolling and branchless programming.
- **Register File Management**: Efficient use of registers through local variables and inlining.
- **Instruction Execution**: Intrinsic functions and bit manipulation for efficient instruction handling.

## Optimization Techniques Included

- **Loop Unrolling**: Reduces loop overhead for better pipeline utilization.
- **Branchless Programming**: Minimizes costly branches and mispredictions.
- **Memory Allocation with `stackalloc`**: Leverages stack memory for small arrays to improve cache locality.
- **Hardware Intrinsics**: Uses SIMD instructions and operations like fused multiply-add (FMA) to reduce latency.
- **Bit Manipulation Hacks**: Efficiently handles bit-level operations using popcount, rotate, and conditional moves.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [BenchmarkDotNet](https://benchmarkdotnet.org/) for benchmarking and measuring performance

### Installation

Clone the repository:

```bash
git clone https://github.com/Hawkynt/PerformanceStudies.git
cd PerformanceStudies
```

### Usage

1. **Run Benchmarks**: To measure the performance of each optimization, execute the benchmark suite:

   ```bash
   dotnet run -c Release
   ```

2. **Explore Code Examples**: Each optimization is contained in its own class with comments explaining the techniques used and their expected performance impact.

## Contributing

Contributions to expand and refine optimization techniques are welcome! Please open a pull request or issue to discuss potential enhancements.

## License

This project is licensed under the LGPL License. See [LICENSE](LICENSE) for more details.
