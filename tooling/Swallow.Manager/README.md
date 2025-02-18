# CLI "manager" for the monorepo

Once `.slnx` files are stable enough, I'll use those going forward.

## Usage
```
swallow generate                   (re-)generate the main solution and all project solutions
swallow add      $NAME             create a new project $NAME and all additional files; implicitly calls generate
swallow publish  $NAME $VERSION    build and publish project $NAME to /publish, tagging it as $VERSION
```

## Repository structure
| Path                                | Description                                                         | Main solution | Project solution |
|-------------------------------------|---------------------------------------------------------------------|---------------|------------------|
| `/assets/`                          | Included binary files - images etc.                                 |               |                  |
| `/lib/`                             | Built (internal) packages to be included in the repository          |               |                  |
| `/shared/`                          | Any shared code snippets to be included as linked files             |               |                  |
| `/.editorconfig`                    | Style & analyzer settings                                           |               |                  |
| `/.gitattributes`                   | Git settings, duh                                                   |               |                  |
| `/icon.png`                         | Main icon to be used by the repository and all packages             |               |                  |
| `/NuGet.config`                     | NuGet config pointing to `/lib` and `nuget.org`                     |               |                  |
| `/README.md`                        | General overview containing all projects and their status / version |               |                  |
| `/Swallow.sln`                      | Main solution including all projects                                |               |                  |
| `/PROJECT/`                         | Subfolder for a specific project                                    |               |                  |
| `/PROJECT/doc/`                     | Additional documentation for this project, markdown and images      |               |                  |
| `/PROJECT/example/`                 | Any examples for usage - never included in the published packages   |               |                  |
| `/PROJECT/src/`                     | All source code that will be published                              |               |                  |
| `/PROJECT/test/`                    | Test project(s) for published packages                              |               |                  |
| `/PROJECT/Directory.Build.props`    | (Templated) project settings - will be duplicated between projects  |               |                  |
| `/PROJECT/Directory.Build.targets`  | (Templated) built targets - will be duplicated between projects     |               |                  |
| `/PROJECT/Directory.Packages.props` | Versions of any used packages, if any                               |               |                  |
| `/PROJECT/LICENSE.txt`              | License, duh                                                        |               |                  |
| `/PROJECT/README.md`                | Readme, duh                                                         |               |                  |
| `/PROJECT/RELEASE_NOTES.md`         | Release notes, duh                                                  |               |                  |
| `/PROJECT/PROJECT.sln`              | Solution containing only this project - nothing else                |               |                  |
