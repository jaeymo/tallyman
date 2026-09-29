# tallyman

A small command-line tool that counts the lines in every file under a directory and prints the results as a table with a running total.

Built with C# and [Spectre.Console](https://spectreconsole.net/).

## Install

### As a .NET global tool

Requires the [.NET SDK](https://dotnet.microsoft.com/download).

From the solution root (the folder containing the `.sln`):

```
dotnet pack src/tallyman.Cli -c Release
dotnet tool install --global --add-source ./src/tallyman.Cli/bin/Release tallyman
```

To update after making changes, bump `<Version>` in `src/tallyman.Cli/tallyman.Cli.csproj`, pack again, then run:

```
dotnet tool update --global --add-source ./src/tallyman.Cli/bin/Release tallyman
```

To uninstall:

```
dotnet tool uninstall --global tallyman
```

## Usage

```
tallyman <directories...>
```

| Argument          | Description                            |
| ----------------- | -------------------------------------- |
| `<directories...>` | One or more directories to count      |

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

| Code | Meaning                                  |
| ---- | ---------------------------------------- |
| `0`  | Success (including when no files found)  |
| `1`  | One of the given directories does not exist |

## Notes

- Subdirectories are searched recursively.
- Every file is counted, including binary files, so pointing it at folders like `bin/`, `obj/`, or `.git/` will give meaningless numbers.
- Colors are only shown in a real terminal. They are stripped when output is redirected (for example `tallyman . > out.txt`) or when the `NO_COLOR` environment variable is set.

## Development

Run from source:

```
dotnet run --project src/tallyman.Cli -- ./some-folder
```

Run the tests:

```
dotnet test
```