# Agent guide — PerformanceStudies

Working agreement for **all** coding agents and human contributors working in
this repository. These rules are not optional. The full house spec lives in
the `Hawkynt/project-template` repo (`STANDARD.md`); this file is the
per-repo distillation.

## What this is

C# **micro-optimization studies** with benchmarks (e.g. `CompareByteArrays`)
plus the accompanying LaTeX paper (`Paper.tex` → `Paper.pdf`). Findings in
the paper and the benchmark code must stay in sync.

## Commits

- **Group changes semantically/logically** — one study/technique per commit.
- **Every subject line starts with a prefix**: `+` added · `-` removed ·
  `*` changed · `#` bug fixed · `!` critical todo.
- Never start a subject with "fix"/"bugfix"/"changed"/"modified".
- **No AI traces anywhere**: no `Co-Authored-By` AI lines, no "Generated
  with" footers, no agent mentions in messages, comments, or authorship.

## The loop (always, in this order)

1. **Before committing**: build the touched study solution(s) in Release
   and run the benchmark to confirm the claimed effect still reproduces —
   a study whose numbers no longer hold is a `#` bug. Regenerate `Paper.pdf`
   from `Paper.tex` when the paper changes (never edit the PDF directly).
2. **Commit** (rules above) and **push**.
3. **Wait for CI**; on `main` a green CI triggers the nightly (prerelease +
   GFS prune, same-day replace). Fix and loop until everything is green.

Stable releases are **manual** (`gh workflow run release.yml`) — never cut
one unless explicitly asked.

## Code conventions

- Benchmarks measure in **Release** with warmup; results cited in README or
  paper name the CPU/runtime they were taken on.
- Each technique gets the naive baseline next to the optimized variant —
  the comparison IS the study.
- Latest C# features; intrinsics-based variants guard with capability checks
  and fall back gracefully.

## README & repo conventions

- Standard frame: title → badges → one-line `>` blockquote (no Overview
  header); fixed emoji mapping for the standard sections (`## ✨ Features`,
  `## 📦 Getting Started`, `## ❤️ Support`, `## 📜 License`).
- License is LGPL-3.0-or-later; the `## ❤️ Support` section and
  `.github/FUNDING.yml` stay intact.
