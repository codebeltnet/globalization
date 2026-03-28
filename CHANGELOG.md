# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 9.0.1 was migrated from previous versions of Cuemon.Extensions.Globalization.

## [10.0.5] - 2026-03-28

This is a patch release that focuses on dependency upgrades across all supported target frameworks, tooling maintenance, and minor workflow refinements.

### Changed

- Upgraded `Codebelt.Extensions.Xunit` from 11.0.7 to 11.0.8,
- Upgraded `Codebelt.Extensions.YamlDotNet` from 10.1.0 to 10.1.1,
- Upgraded `coverlet.collector` and `coverlet.msbuild` to 8.0.1,
- Upgraded `xunit.v3` and `xunit.v3.runner.console` to 3.2.2,
- Upgraded `xunit.runner.visualstudio` to 3.1.5,
- Upgraded docfx Docker image from 2.78.4 to 2.78.5,
- Added `carter` repository mapping in the NuGet bump script,
- Fixed trailing-whitespace in service-update workflow entry template.

## [10.0.4] - 2026-02-28

This is a service update that focuses on package dependencies.

## [10.0.3] - 2026-02-20

This is a service update that focuses on package dependencies.

### Fixed

- Added embedded resources for these missed cultures: `bgc-deva-in`, `bho-deva-in`, `cv-cyrl-ru`, `en-mv`, `hi-latn-in`, `kgp-latn-br`, `oc-es`, `raj-deva-in`, `sc-latn-it`, `yrl-latn-br`, `yrl-latn-co` and `yrl-latn-ve`

## [10.0.2] - 2026-02-15

This is a service update that focuses on package dependencies.

## [10.0.1] - 2026-01-22

This is a service update that focuses on package dependencies.

## [10.0.0] - 2025-11-13

This is a major release that focuses on adapting the latest `.NET 10` release (LTS) in exchange for current `.NET 8` (LTS).

> To ensure access to current features, improvements, and security updates, and to keep the codebase clean and easy to maintain, we target only the latest long-term (LTS), short-term (STS) and (where applicable) cross-platform .NET versions.

## [9.0.8] - 2025-10-20

This is a service update that focuses on package dependencies.

## [9.0.7] - 2025-09-15

This is a service update that focuses on package dependencies.

## [9.0.6] - 2025-08-19

This is a service update that focuses on package dependencies.

## [9.0.5] - 2025-07-11

This is a service update that focuses on package dependencies.

## [9.0.4] - 2025-06-15

This is a service update that focuses on package dependencies.

## [9.0.3] - 2025-05-25

This is a service update that focuses on package dependencies.

## [9.0.2] - 2025-04-16

This is a service update that focuses on package dependencies.

## [9.0.1] - 2025-01-29

This is a service update that primarily focuses on package dependencies and minor improvements.

## [8.4.0] - 2024-09-28

### Changed

- CultureInfoExtensions class in the Codebelt.Extensions.Globalization namespace to use Codebelt.Extensions.YamlDotNet assembly instead of YAML support from Cuemon.Extensions.YamlDotNet assembly

## [8.3.0] - 2024-04-09

### Changed

- CultureInfoExtensions class in the Codebelt.Extensions.Globalization namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- CultureInfoSurrogate class in the Codebelt.Extensions.Globalization namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- tooling/gse to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly 

## [8.0.0] - 2023-11-14

### Added

- Tool for extracting NLS surrogates `tooling/gse` (Globalization Surrogates Extractor); this was done to mitigate the original design decision that was most [unfortunate](https://github.com/gimlichael/Cuemon/commit/71ff4f9ecb95897170aab1e6ba894c320ae095bd)

### Fixed

- National Language Support (NLS) surrogates was updated in the Codebelt.Extensions.Globalization assembly

## [7.0.0] 2022-11-09

### Added

- CultureInfoExtensions class in the Codebelt.Extensions.Globalization namespace that consist of extension methods for the CultureInfo class: UseNationalLanguageSupport

[Unreleased]: https://github.com/codebeltnet/globalization/compare/v10.0.5...HEAD
[10.0.5]: https://github.com/codebeltnet/globalization/compare/v10.0.4...v10.0.5
[10.0.4]: https://github.com/codebeltnet/globalization/compare/v10.0.3...v10.0.4
[10.0.3]: https://github.com/codebeltnet/globalization/compare/v10.0.2...v10.0.3
[10.0.2]: https://github.com/codebeltnet/globalization/compare/v10.0.1...v10.0.2
[10.0.1]: https://github.com/codebeltnet/globalization/compare/v10.0.0...v10.0.1
[10.0.0]: https://github.com/codebeltnet/globalization/compare/v9.0.8...v10.0.0
[9.0.8]: https://github.com/codebeltnet/globalization/compare/v9.0.7...v9.0.8
[9.0.7]: https://github.com/codebeltnet/globalization/compare/v9.0.6...v9.0.7
[9.0.6]: https://github.com/codebeltnet/globalization/compare/v9.0.5...v9.0.6
[9.0.5]: https://github.com/codebeltnet/globalization/compare/v9.0.4...v9.0.5
[9.0.4]: https://github.com/codebeltnet/globalization/compare/v9.0.3...v9.0.4
[9.0.3]: https://github.com/codebeltnet/globalization/compare/v9.0.2...v9.0.3
[9.0.2]: https://github.com/codebeltnet/globalization/compare/v9.0.1...v9.0.2
[9.0.1]: https://github.com/codebeltnet/globalization/compare/v8.4.0...v9.0.1
[8.4.0]: https://github.com/codebeltnet/globalization/compare/v8.3.0...v8.4.0
[8.3.0]: https://github.com/codebeltnet/globalization/compare/v8.0.0...v8.3.0
[8.0.0]: https://github.com/codebeltnet/globalization/compare/v7.0.0...v8.0.0
[7.0.0]: https://github.com/codebeltnet/globalization/releases/tag/v7.0.0
