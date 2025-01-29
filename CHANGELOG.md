# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 9.0.1 was migrated from previous versions of Cuemon.Extensions.Globalization.

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
