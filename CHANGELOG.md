# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.4.0] / 2023-01-31
### Features
- `ricaun.Revit.Github` without Revit Reference.
- Change to .NET Framework 4.5 
- Force dependence in `Newtonsoft.Json`
- Improve `Initialize` documentation

## [0.3.0] / 2023-01-31
### Features
- [x] Add `Directory.Build.props` one file change all the versions with `Condition`
- [x] Add Example Command ToolTip with Version
- [x] Initialize only download with greater Version and return true if download
- [x] Update `CommandUpdate` with `ShowBalloon`
### Added
- Add `InfoCenterUtil` with `ShowBalloon`

## [0.2.0] / 2023-01-31
### Features
- [x] Initialize Auto Download Last Version - `Console log`
- [x] Update Example Project to Update Last Version

## [0.1.0] / 2022-05-21
### Features
- [x] Download Bundle From Github
- [ ] Improve Library Files
- [ ] Select Download Version
#### GithubRequestService
- [x] Clear
- [x] Remove Github Data 
- [x] Add PathBundle
- [x] Add XML comment
### Fixed
- Fix Async Methods - Problem
### Added
- Add Async in all Services
- Add BundleModel (Version, DownloadUrl, Body, DataTime)
- Add DownloadBundleService
- Add GithubBundleService
- Add GithubService
- Add JsonService
- Add PathBundleService
- Add GithubRequestService
- Base Package Files
- Add Example

[vNext]: ../../compare/1.0.0...HEAD
[0.4.0]: ../../compare/0.3.0...0.4.0
[0.3.0]: ../../compare/0.2.0...0.3.0
[0.2.0]: ../../compare/0.1.0...0.2.0
[0.1.0]: ../../compare/0.1.0
[1.0.0]: ../../compare/1.0.0