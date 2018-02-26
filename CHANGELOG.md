# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [1.0.0] - 2017-12-14
### Added
- Initial version released forked from https://github.com/jakubka/swashbuckle-nodatime
	Supports NodaTime on Swashbuckle.AspNetCore on .NET 4.5.1 and .NET Standard 1.6 (or higher)

## [1.0.1] - 2018-02-24
### Changed
- Sample schema now pulls in users local time zone rather than defaulting to America/New_York
- Tested against the 2.1.0 release of Swashbuckle.AspNetCore

## [1.1.0] - 2018-02-25
### Added
- Format now matches RFC3339 types when possible https://www.ietf.org/rfc/rfc3339.txt
  - Instant: date-time
  - LocalDate: full-date
  - LocalTime: partial-time (Partial due to lack of offset)
  - Offset: time-numoffset (UTC offset serializes as "+00" and not "Z")
  - OffsetDateTime: full-date

## [1.1.1] - 2018-02-25 
### Changed
- Sample schema now attempts to use users local timezone but if not found safely falls back to America/New_York
- Fixed the access control on the API to only expose the two configuration extension methods everything is internal

## [1.1.2] - 2018-02-26
## Changed
- Something went wrong with the nuget.org upload it's still validating the package some 13+ hours later sorry for the version spam this should be the final release for a good long while I have fixed everything I wanted to fix this round
