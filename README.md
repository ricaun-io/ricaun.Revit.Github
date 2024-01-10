# ricaun.Revit.Github

[![Revit 2017](https://img.shields.io/badge/Revit-2017+-blue.svg)](../..)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](../../actions/workflows/Build.yml/badge.svg)](../../actions)
[![Release](https://img.shields.io/nuget/v/ricaun.Revit.Github?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Revit.Github)

The `ricaun.Revit.Github` allows downloading bundle files in Autodesk format from Github and unpacking the `zip` in the installed bundle folder.

The bundle file is generated and released using the [ricaun.Nuke.PackageBuilder](https://github.com/ricaun-io/ricaun.Nuke.PackageBuilder) build automation with Github Action.

This project was generated by the [AppLoader](https://ricaun.com/apploader/) Revit plugin.

## GithubRequestService

The `GithubRequestService(user, repo)` contains all the logic to download and unpack the bundle files in the release section from the `user` and `repo` in Github.

```C#
var service = new GithubRequestService("ricaun-io", "ricaun.Revit.Github");
```

### Initialize

The `Initialize` method starts the process to read the content in the release section in Github and find the last version that is greater than the current version of the plugin.
This method only works if the plugin is installed using the bundle format.

```C#
var service = new GithubRequestService("ricaun-io", "ricaun.Revit.Github");
bool downloadedNewVersion = await service.Initialize();
```

## ricaun.Revit.Github.Example

This is a simple RevitAddin project implementation to test the `ricaun.Revit.Github` library, the command force to download the last version in this repository.

## Release

* Download and install [ricaun.Revit.Github.Example.exe](../../releases/latest/download/ricaun.Revit.Github.Example.zip)
* [Latest release](../../releases/latest)

## Videos

Live videos in portuguese with the creation of this project.

[![VideoIma1]][Video1]
[![VideoIma2]][Video2]
[![VideoIma3]][Video3]
[![VideoIma4]][Video4]
[![VideoIma5]][Video5]

## License

This project is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!

[Video1]: https://youtu.be/KVVhb15DMrc
[VideoIma1]: https://img.youtube.com/vi/KVVhb15DMrc/mqdefault.jpg
[Video2]: https://youtu.be/Io_qCFBLJ-0
[VideoIma2]: https://img.youtube.com/vi/Io_qCFBLJ-0/mqdefault.jpg
[Video3]: https://youtu.be/ozbPewoPi9g
[VideoIma3]: https://img.youtube.com/vi/ozbPewoPi9g/mqdefault.jpg
[Video4]: https://youtu.be/2hFsxYJapOc
[VideoIma4]: https://img.youtube.com/vi/2hFsxYJapOc/mqdefault.jpg
[Video5]: https://youtu.be/_KaACIOmpGA
[VideoIma5]: https://img.youtube.com/vi/_KaACIOmpGA/mqdefault.jpg