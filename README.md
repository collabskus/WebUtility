# WebUtility

A lightweight .NET 10 utility library providing common web and business-logic helpers. Currently ships a `Calculator` class with fiscal year computation; designed to grow as a shared foundation across projects.

[![CI](https://github.com/collabskus/WebUtility/actions/workflows/ci.yml/badge.svg)](https://github.com/collabskus/WebUtility/actions/workflows/ci.yml)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL_v3-blue.svg)](https://www.gnu.org/licenses/agpl-3.0)

---

## Table of Contents

- [What it does](#what-it-does)
- [Why it exists](#why-it-exists)
- [Requirements](#requirements)
- [Getting started](#getting-started)
- [API reference](#api-reference)
- [Running the tests](#running-the-tests)
- [Project structure](#project-structure)
- [Technology choices](#technology-choices)
- [Contributing](#contributing)
- [AI assistance disclaimer](#ai-assistance-disclaimer)
- [License](#license)

---

## What it does

### `Calculator.CalculateFiscalYear(DateTime)`

Returns the **US federal fiscal year** for a given date.

The US federal fiscal year runs from **October 1 through September 30**. A date in October 2024 belongs to FY 2025; a date in September 2024 belongs to FY 2024.

```csharp
using WebUtility;

Calculator.CalculateFiscalYear(new DateTime(2024, 9, 30)); // → 2024
Calculator.CalculateFiscalYear(new DateTime(2024, 10, 1)); // → 2025
Calculator.CalculateFiscalYear(DateTime.MinValue);         // → 0  (sentinel / not applicable)
```

| Input month | Returns |
|-------------|---------|
| January–September | `date.Year` |
| October–December | `date.Year + 1` |
| `DateTime.MinValue` | `0` |

---

## Why it exists

Business logic for fiscal year calculation is simple enough that developers often re-implement it inline, which leads to subtle inconsistencies (off-by-one on the October boundary, disagreement on `DateTime.MinValue` handling, etc.). This library pins a single canonical implementation, thoroughly tested, that can be pulled in as a project reference or NuGet package rather than copy-pasted.

It was spun out from a larger internal solution specifically to be independently versioned and reusable.

---

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Visual Studio 2022 17.13+ or Rider 2024.3+ (for IDE test runner support with MTP)

No runtime dependencies beyond the BCL.

---

## Getting started

### As a project reference

Clone the repo and add a project reference to `WebUtility/WebUtility.csproj`:

```xml
<ProjectReference Include="../WebUtility/WebUtility.csproj" />
```

### Clone and build locally

```bash
git clone https://github.com/collabskus/WebUtility.git
cd WebUtility
dotnet build
```

---

## API reference

### `WebUtility.Calculator`

```csharp
public static class Calculator
```

#### `CalculateFiscalYear`

```csharp
public static int CalculateFiscalYear(DateTime inputDate)
```

**Parameters**

| Name | Type | Description |
|------|------|-------------|
| `inputDate` | `DateTime` | The date to compute the fiscal year for. `DateTimeKind` is ignored; only year and month matter. |

**Returns**

`int` — The US federal fiscal year number, or `0` if `inputDate` is `DateTime.MinValue`.

**Remarks**

- The fiscal year boundary is **October 1**. Dates before October belong to the same calendar year; October onwards advance to the next calendar year.
- `DateTime.MinValue` (`0001-01-01`) is treated as a sentinel "no date" value and returns `0` rather than `1`, which would be the technically correct fiscal year for that date.
- The method is pure and stateless. Calling it multiple times with the same input always produces the same result.

---

## Running the tests

This project uses **xUnit v3** with the **Microsoft Testing Platform v2 (MTP)** runner. MTP replaces the legacy VSTest pipeline on .NET 10 and is the required adapter for `dotnet test` compatibility.

```bash
# Run all tests
dotnet test --configuration Release

# Run with coverage and TRX report
dotnet test --configuration Release -- --report-trx --coverage --coverage-output-dir TestResults/Coverage
```

> **Visual Studio users:** Ensure you are on VS 2022 17.13 or later. The `<TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>` property in the test project enables the Test Explorer to discover MTP-based tests correctly.

### Test coverage

The test suite covers:

- `DateTime.MinValue` sentinel handling (four variants covering `DateTimeKind`)
- The fiscal boundary: September 30 → FY N, October 1 → FY N+1, for every year 0001–9999
- January and December across a range of years
- Return type correctness
- Determinism / idempotency
- Full domain sweep across all months for all years (first of month)
- Randomised stress test (100 000 iterations, fixed seed for reproducibility)
- High-frequency invocation stability (1 000 000 calls)
- Result always in valid range [0, 10000]
- Result is always either `year` or `year + 1`
- `DateTimeKind` does not affect the result

---

## Project structure

```
WebUtility/
├── Directory.Packages.props        # Central package version management
├── WebUtility.slnx                 # Solution file
│
├── WebUtility/
│   ├── WebUtility.csproj
│   └── Calculator.cs               # Core library code
│
└── WebUtility.Tests/
    ├── WebUtility.Tests.csproj
    └── CalculatorTests.cs          # xUnit v3 test suite
```

---

## Technology choices

| Choice | Rationale |
|--------|-----------|
| **.NET 10** | Latest LTS-bound release; enables modern C# features and the new MTP test pipeline |
| **xUnit v3** | The current major version; v3 is a ground-up rewrite with better parallelism and MTP integration |
| **xunit.v3.mtp-v2** | Replaces the old `xunit.runner.visualstudio` + `Microsoft.NET.Test.Sdk` pair; required on .NET 10 where VSTest is no longer supported |
| **Central Package Management** | All package versions live in `Directory.Packages.props`; individual projects reference packages without version attributes, preventing version drift across a multi-project solution |
| **`CentralPackageTransitivePinningEnabled`** | Pins transitive dependency versions explicitly, improving reproducibility |
| **coverlet** | Cross-platform code coverage collector integrated with the MTP pipeline |

---

## Contributing

1. Fork the repo and create a feature branch.
2. Make your changes with tests.
3. Ensure `dotnet test` passes on your machine.
4. Open a pull request against `main`. CI will run on Linux, Windows, and macOS automatically.

Please keep pull requests focused. If you are adding a new utility, open an issue first to discuss the design.

---

## AI assistance disclaimer

Portions of this project — including code, tests, configuration, documentation, and CI pipeline — were developed with the assistance of AI language models, specifically **Claude** (Anthropic) and **ChatGPT** (OpenAI). AI-generated content was reviewed, validated, and edited by human contributors before being committed. The use of these tools accelerated development but all design decisions, logic correctness, and final content remain the responsibility of the project maintainers.

---

## License

This project is licensed under the **GNU Affero General Public License v3.0 (AGPL-3.0)**.

See [LICENSE](LICENSE) for the full text, or visit [https://www.gnu.org/licenses/agpl-3.0.html](https://www.gnu.org/licenses/agpl-3.0.html).

In summary: you are free to use, modify, and distribute this software, but any modified version — including when run as a networked service — must also be released under AGPL-3.0 with its source code made available.
