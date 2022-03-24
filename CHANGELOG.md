# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [4.0.0] - 2022-03-24

### Changed

- Removed NewtonSoft.Json in favour of System.Text.Json (mostly becuase the current versions of Swashbuckle are using System.Text.Json)
- Upgraded to the current latest stable release of NodaTime - v3.0.10
- Switched from Swashbuckle.WebApi to latest version of Swashbuckle.AspNetCore - v6.3.0
- Switched to xunit (v2.4.2) for unit testing

## [3.0.0] - 2017-07-29

### Changed

- Upgraded to be compatible with the latest stable release of NodaTime - 2.0.0 (thanks @CodeRevver)

## [2.2.0] - 2017-04-01

### Changed

- Removed unnecessary dependencies (thanks @GlosSoft)

## [2.1.0] - 2017-04-01

### Added

- Nullable types (such as `Instant?`, `LocalDate?`, etc.) are now correctly displayed in Swagger (thanks @GlosSoft)

## [2.0.1] - 2017-04-01

### Added

- AppVeyor is now used to build, test and release the package

## [2.0.0] - 2016-02-28

### Fixed

- fixed bug with double quotes in swagger examples

### Breaking changes

- it is now required to provide `JsonSerializerSettings` when configuring swagger for `NodaTime`

## [1.0.0] - 2016-02-13

### Added

- Initial version released
