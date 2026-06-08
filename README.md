# PerformanceStudies

[![License](https://img.shields.io/github/license/Hawkynt/PerformanceStudies)](https://github.com/Hawkynt/PerformanceStudies/blob/main/LICENSE)
[![Language](https://img.shields.io/github/languages/top/Hawkynt/PerformanceStudies?color=8957D5)](https://github.com/Hawkynt/PerformanceStudies)

[![CI](https://github.com/Hawkynt/PerformanceStudies/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Hawkynt/PerformanceStudies/actions/workflows/ci.yml)
![Last Commit](https://img.shields.io/github/last-commit/Hawkynt/PerformanceStudies?branch=main)
![Activity](https://img.shields.io/github/commit-activity/m/Hawkynt/PerformanceStudies)

[![Stars](https://img.shields.io/github/stars/Hawkynt/PerformanceStudies?color=FFD700)](https://github.com/Hawkynt/PerformanceStudies/stargazers)
[![Forks](https://img.shields.io/github/forks/Hawkynt/PerformanceStudies?color=008080)](https://github.com/Hawkynt/PerformanceStudies/network/members)
[![Issues](https://img.shields.io/github/issues/Hawkynt/PerformanceStudies)](https://github.com/Hawkynt/PerformanceStudies/issues)
![Code Size](https://img.shields.io/github/languages/code-size/Hawkynt/PerformanceStudies?color=4CAF50)
![Repo Size](https://img.shields.io/github/repo-size/Hawkynt/PerformanceStudies?color=FF9800)

[![Release](https://img.shields.io/github/v/release/Hawkynt/PerformanceStudies)](https://github.com/Hawkynt/PerformanceStudies/releases/latest)
[![Nightly](https://img.shields.io/github/v/release/Hawkynt/PerformanceStudies?include_prereleases&sort=date&filter=nightly-*&label=nightly&color=FF9800)](https://github.com/Hawkynt/PerformanceStudies/releases)
[![Downloads](https://img.shields.io/github/downloads/Hawkynt/PerformanceStudies/total)](https://github.com/Hawkynt/PerformanceStudies/releases)

> A C# repository demonstrating and measuring low-level performance optimizations ([Paper](Paper.pdf)) — cache locality, pipeline utilization, register management and instruction-level tricks, each with benchmarks and an explanation of why it wins.

## ✨ Features

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

## 📦 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [BenchmarkDotNet](https://benchmarkdotnet.org/) for benchmarking and measuring performance

### Installation

Clone the repository:

```bash
git clone https://github.com/Hawkynt/PerformanceStudies
cd PerformanceStudies
```

### Usage

1. **Run Benchmarks**: To measure the performance of each optimization, execute the benchmark suite:

   ```bash
   dotnet run -c Release
   ```

2. **Explore Code Examples**: Each optimization is contained in its own class with comments explaining the techniques used and their expected performance impact.

## 🤝 Contributing

Contributions to expand and refine optimization techniques are welcome! Please open a pull request or issue to discuss potential enhancements.

## ❤️ Support

If this project saves you time or money, consider supporting its development:

[![GitHub Sponsors](https://img.shields.io/badge/GitHub-Sponsor-EA4AAA?logo=githubsponsors)](https://github.com/sponsors/Hawkynt)
[![PayPal](https://img.shields.io/badge/PayPal-Donate-00457C?logo=paypal)](https://www.paypal.me/hawkynt)

## 📜 License

Licensed under LGPL-3.0-or-later — see [LICENSE](LICENSE).
