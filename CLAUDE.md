# Project rules for Claude

## What this is

DataTable2DataGridViewExample is a small Windows Forms example that shows how the contents of a
`DataTable` reach a `DataGridView`: build the table in code, assign it to `DataGridView.DataSource`,
done. It is a code example, **not** a library and **not** a NuGet package: no `OutputType`
`Library`, no `GeneratePackageOnBuild`, no push script. There is also no installer, no `Setup`
folder and no CI file in the repository.

One solution `src/DataTable2DataGridViewExample.sln` with exactly one project:

- `src/DataTable2DataGridViewExample/DataTable2DataGridViewExample.csproj`, `OutputType` `WinExe`,
  `UseWindowsForms`, `TargetFramework` `net10.0-windows`, `RuntimeIdentifiers` `win-x64`,
  `ApplicationIcon` `Grid.ico`.

Layout inside `src/DataTable2DataGridViewExample`:

- `Program.cs`: the entry point. `[STAThread] public static void Main()` with
  `ApplicationConfiguration.Initialize()` and `Application.Run(new Main())`. There is no
  `StartupObject` property, the SDK finds this `Main` because it is the only one.
- `Main.cs`: the form. The constructor only calls `InitializeComponent`, all example code sits in
  the `FormLoad` handler: set the window title, create the `DataTable`, add four columns, make every
  column required, add a `UniqueConstraint` over first and last name, add four rows, assign the
  table as `DataSource`.
- `Main.Designer.cs` and `Main.resx`: designer output. The `.resx` holds no resources, only the
  default headers.
- `GlobalUsings.cs`: the single using of the project, `global using System.Data;`.
- `Grid.ico`: the application icon. `License.txt`: a copy of the root license, copied to the output
  directory with `CopyToOutputDirectory=Always`.

Repository root: `README.md` (the only user documentation, spelled in capitals unlike the
`Readme.md` of the sibling repositories), `Changelog.md`, `License.txt` (MIT), `.gitattributes` and
`.gitignore`. The `.editorconfig` lives in `src`, not in the root. There is no `Updating.md`, no
`HowToUse.md` and no screenshots.

## Build

```powershell
dotnet build src/DataTable2DataGridViewExample.sln
```

- Single target framework `net10.0-windows`, no multi-targeting. Windows only, the project is a
  Windows Forms application.
- All build properties live directly in the single `.csproj`. There is **no**
  `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.8-1` for the first
  commit after tag `1.0.7`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/DataTable2DataGridViewExample.sln --source https://api.nuget.org/v3/index.json`.
- There are no tests. A behaviour change is verified by starting the application and looking at the
  grid: four columns, four rows, `Salary` right aligned as a number, and the window title showing
  the version.

## Code conventions

Follow the surrounding code, it is consistent in the hand written files:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `src/DataTable2DataGridViewExample/GlobalUsings.cs`, inside the
  existing `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig
  requires usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`),
  which global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- `Main.Designer.cs` follows none of this: block scoped namespace, no `this.` on `components`,
  German doc comments, and it is exempt because the designer rewrites the file. Do not hand tune it
  beyond what the designer would write itself.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The window title carries the version.** The designer sets `Text` to
  `DataTable2DataGridViewExample`, `FormLoad` overwrites it with
  `$@"{Application.ProductName} {Application.ProductVersion}"`. `ProductVersion` is the
  informational version from GitVersion, so an untagged build shows something like
  `1.0.8-1+Branch.master.Sha...` in the title bar. Same behaviour as the sibling repository
  `512kbChecker`.
- **The null checks on the columns are not paranoia.** `DataTable.Columns["First Name"]` returns
  `DataColumn?`, and warnings are errors here, so `Main.FormLoad` collects the two columns in a
  `List<DataColumn>` behind `is not null` checks before it builds the `UniqueConstraint`. The
  columns are added three lines above and can never be null, the checks only satisfy the nullable
  analysis. Replacing them with `!` would work but nothing else in these repositories uses `!`.
- **The constraints are part of the example.** Every column gets `AllowDBNull = false` and first
  plus last name get a `UniqueConstraint`. The `DataGridView` inherits both, so editing a cell or
  adding a row in the running application can raise `NoNullAllowedException` or
  `ConstraintException`. There is no `DataError` handler, the grid shows its own default error
  dialog. That is what the example demonstrates, it is not a bug to hide.
- **The sample data is quoted from the original example.** Rod Stephens, Sergio Aragones, Eoin
  Colfer and Terry Pratchett with salaries 10000 to 40000. The names are also the reason why the
  ReSharper user dictionary knows `Aragones`, `Colfer` and `Eoin`.
- **`RuntimeIdentifiers win-x64` without anything that publishes.** The property is set although
  the repository never runs `dotnet publish` and has no installer. It costs nothing, restore just
  resolves the RID specific assets as well.
- **`README.md` in capitals.** The sibling repositories use `Readme.md`. Renaming it would break
  every link that points at the file on GitHub.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`src/DataTable2DataGridViewExample.sln.DotSettings`** is tracked and holds nothing but a
  ReSharper user dictionary (`Aragones`, `Colfer`, `Eoin`, `H_00E4mmer`). Leave it alone.
- **`.gitattributes` sets `* text=auto`** and every rule of the Visual Studio template below it is
  commented out. Any binary file added later needs its own rule, otherwise git decides by heuristic
  whether its line endings get normalized.
- **`License.txt` exists twice**, in the root and in the project, byte for byte identical. The
  project copy is the one that ends up next to the executable.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-11)** : Short description.`
3. Commit that.
4. Tag the commit with the plain version number, no `v` prefix (`1.0.7`, `1.0.6`, ...). The existing
   tags are lightweight tags, create new ones the same way.
5. Push the commits and the tag.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.8-1+Branch.master.Sha...`. There is no installer to build and no package to push, so the
release ends with the push.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
