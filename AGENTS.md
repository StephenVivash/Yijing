# Repository Guidelines

## Project Structure & Module Organization

This repository is organized around `Yijing.sln`, a .NET 10 solution. Core sequencer logic lives in `ValueSequencer/`, with ports in `value_sequencer_py/` and `ValueSequencer.ts/`. Application projects include `Yijing.maui/` for the MAUI client and `Yijing.web/` for the Blazor web app. Data access and migrations are split between `Yijing.data/` and `Yijing.db/`. EEG and Cortex-related code is in `EegML/` and `CortexAccess/`. Tests are in `Yijing.test/` and `Yijing.maui.test/`. App assets, fonts, raw sample data, and audio files are under `Yijing.maui/Resources/`.

## Build, Test, and Development Commands

- `dotnet restore Yijing.sln` restores NuGet packages for the solution.
- `dotnet build Yijing.sln` builds all solution projects. The solution references shared projects under `..\Common`, so ensure that sibling directory exists.
- `dotnet test Yijing.test/Yijing.test.csproj` runs sequencer unit tests.
- `dotnet test Yijing.maui.test/Yijing.maui.test.csproj` runs MAUI-related tests.
- `dotnet run --project Yijing.web/Yijing.web.csproj` starts the Blazor web app locally.
- `tsc -p ValueSequencer.ts/tsconfig.json` type-checks the TypeScript sequencer port when TypeScript is installed.

## Coding Style & Naming Conventions

Follow `.editorconfig`: tabs with width 4, CRLF line endings, UTF-8, final newline, and no trailing whitespace. C# uses nullable reference types and implicit usings in current test projects. Prefer block-scoped namespaces, braces, PascalCase for types and public members, and `I` prefixes for interfaces. Keep sequencer class names aligned across language ports where practical, for example `CHexagramValueSequencer`.

## Testing Guidelines

Tests use xUnit v3 with `Microsoft.NET.Test.Sdk` and `coverlet.collector`. Add tests near the behavior being changed: general sequencer invariants belong in `Yijing.test/`, while UI or MAUI behavior belongs in `Yijing.maui.test/`. Name test files after the subject, such as `HexagramTests.cs`, and use descriptive test method names that state the expected behavior.

## Commit & Pull Request Guidelines

Recent commits use short imperative summaries such as `Update Nuget`, `Use Common`, and `Prepare Yijing release`. Keep commits focused and use a clear verb phrase. Pull requests should describe the change, list validation performed, link any related issue, and include screenshots or recordings for MAUI or web UI changes.

## Security & Configuration Tips

Do not commit user-specific IDE state, secrets, generated build outputs, or local database files. Keep credentials and device-specific Cortex or EEG settings outside source control, and prefer app configuration or user secrets for local overrides.
