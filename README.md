# tallyman

A small command-line tool that counts the lines in every file under one or more directories.

## Download

Tallyman is published on NuGet as a .NET global tool. It requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```
dotnet tool install --global tallyman
```

Check that it works:

```
tallyman --help
```

Update to the latest version:

```
dotnet tool update --global tallyman
```

Uninstall:

```
dotnet tool uninstall --global tallyman
```

## Usage

```
tallyman <directories...>
```

| Argument           | Description                      |
| ------------------ | -------------------------------- |
| `<directories...>` | One or more directories to count |

### Examples

Count lines in the current folder:

```
tallyman .
```

Count lines in a specific project:

```
tallyman ~/projects/my-app
```

Count lines from multiple directories:

```
tallyman ./Packages ./src
```

### Output

Each directory gets its own table, with files listed alphabetically and paths relative to that directory, followed by a footer with the file count and total lines. When you pass more than one directory, a grand total is printed at the end.

### Exit codes

| Code | Meaning                                     |
| ---- | ------------------------------------------- |
| `0`  | Success (including when no files found)     |
| `1`  | One of the given directories does not exist |

### Notes

- Subdirectories are searched recursively.
- Every file is counted, including binary files, so pointing it at folders like `bin/`, `obj/`, or `.git/` will give meaningless numbers.
- If one path is inside another (for example `tallyman . ./src`), the overlapping files are counted twice.

## Source code

If you want to contribute, here is how you set up the dev environment:

```
git clone https://github.com/YOUR-USERNAME/tallyman.git
cd tallyman
```

### Run from source

You don't need to install anything to try it; from the repository root (the folder containing the `.sln`):

```
dotnet run --project src/tallyman.Cli -- ./some-folder
```

Everything after the `--` is passed to tallyman.

### Run the tests

```
dotnet test
```

### Build and install your own copy

To install a local build as the `tallyman` command, replacing the NuGet version if you have it:

```
dotnet pack src/tallyman.Cli -c Release
dotnet tool uninstall --global tallyman
dotnet tool install --global --add-source ./src/tallyman.Cli/bin/Release tallyman
```

After making changes, bump `<Version>` in `src/tallyman.Cli/tallyman.Cli.csproj` before packing again, or the tool will keep using the cached older package. Then update with:

```
dotnet tool update --global --add-source ./src/tallyman.Cli/bin/Release tallyman
```

### Standalone executable

To build a single executable that runs without the .NET SDK installed:

```
dotnet publish src/tallyman.Cli -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

Swap `win-x64` for `linux-x64`, `osx-arm64`, or `osx-x64` as needed. The output goes to `src/tallyman.Cli/bin/Release/net10.0/<rid>/publish/`.

## License

MIT