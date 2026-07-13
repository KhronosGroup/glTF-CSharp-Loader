# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0]

### Added
- GitHub Actions CI that builds and tests on every push and pull request.
- Automated NuGet publishing: preview packages (`X.Y.Z-preview.<run>`) on push to `main`, and manual preview/release publishing via workflow dispatch.

### Changed
- **Breaking:** Migrated JSON serialization from Newtonsoft.Json to System.Text.Json. Extension values in `Extensions` are now `System.Text.Json.JsonElement` rather than Newtonsoft `JObject`.
- **Breaking:** Target frameworks are now `netstandard2.0` and `net8.0` (previously `netstandard1.3`).
- Schema classes now share common base classes (`GltfProperty`, `GltfChildOfRootProperty`).
- Mime-type detection from image URIs is now case-insensitive.
- Unit tests load models from the `glTF-Sample-Assets` repository (previously the deprecated `glTF-Sample-Models`).

## [1.1.4-alpha]
- Last version published to NuGet, using Newtonsoft.Json and targeting `netstandard1.3`.
