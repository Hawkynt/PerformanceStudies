# CI/CD Pipeline — PerformanceStudies

Event-driven pipeline (no cron). Workflows live here; their helper scripts live
in `scripts/`.

| File | Trigger | Purpose |
|------|---------|---------|
| `ci.yml` | push + PR on `main` + `workflow_call` | Build the solution on windows (multi-targets net48 + net8.0) |
| `release.yml` | **manual dispatch** | Build the app, then cut the dated `vyyyyMMdd` Release |
| `nightly.yml` | successful CI on `main` + manual | Publish `nightly-yyyyMMdd` prerelease and prune old ones |
| `_build.yml` | `workflow_call` (internal) | Publish the self-contained net8.0 Windows app zip |
| `scripts/version.pl` | invoked by workflows | Stamp each project's own `<Version>` + its folder's commit count (`--stamp`) |
| `scripts/update-changelog.mjs` | invoked by workflows | Bucketise commits into release notes by `+ - * # !` prefix |
| `scripts/prune-nightlies.mjs` | invoked by workflows | GFS retention: 7 daily + 4 weekly + 3 monthly |

## Notes

- **Versioning — files drive, never tags.** The app (`CompareByteArrays`) carries
  its own `<Version>`; `version.pl --stamp` appends its folder's commit count.
  The repo-level Release/tag is the date marker `vyyyyMMdd`.
- **Multi-target build.** The app targets `net48;net8.0`, so CI builds on
  `windows-latest` (it ships both the .NET Framework 4.8 targeting pack and the
  .NET 8 SDK). Releases publish the `net8.0` flavor as a self-contained
  single-file `win-x64` binary.
- **Behaviour change vs the old `Build.yml`:** publishing moved from a weekly
  cron to a manual dispatch; nightlies and changelog notes are automatic.
