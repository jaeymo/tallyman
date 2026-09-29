using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

public class CountSettings : CommandSettings
{
    [CommandArgument(0, "<directories>")]
    [Description("One or more directories to count")]
    public string[] Directories { get; init; } = [];
}

public class CountCommand : Command<CountSettings>
{
    protected override int Execute(
        CommandContext context,
        CountSettings settings,
        CancellationToken cancellationToken)
    {
        foreach (string dir in settings.Directories)
        {
            if (!Directory.Exists(dir))
            {
                AnsiConsole.MarkupLine(
                    $"[red]✗ Directory not found:[/] [yellow]{Markup.Escape(dir)}[/]"
                );
                return 1;
            }
        }

        int grandFiles = 0;
        int grandLines = 0;

        foreach (string dir in settings.Directories)
        {
            (int files, int lines) = CountDirectory(dir, cancellationToken);
            grandFiles += files;
            grandLines += lines;
        }

        if (settings.Directories.Length > 1)
        {
            AnsiConsole.Write(new Rule("[bold]Grand total[/]").LeftJustified().RuleStyle("grey"));
            AnsiConsole.MarkupLine(
                $"[bold green]{grandLines:N0}[/] lines across [bold]{grandFiles:N0}[/] files in [bold]{settings.Directories.Length}[/] directories"
            );
        }

        AnsiConsole.MarkupLine("[green]✓ Done[/]");
        return 0;
    }

    private static (int Files, int Lines) CountDirectory(string directory, CancellationToken cancellationToken)
    {
        string rootDirectory = Path.GetFullPath(directory);

        string[] files = Directory
            .EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories)
            .OrderBy(file => file)
            .ToArray();

        AnsiConsole.Write(
            new Rule($"[bold yellow]tallyman[/] [grey]{Markup.Escape(rootDirectory)}[/]")
                .LeftJustified()
                .RuleStyle("grey")
        );

        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine("[grey]No files found.[/]");
            return (0, 0);
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]File[/]")
            .AddColumn(new TableColumn("[bold]Lines[/]").RightAligned());

        int total = 0;

        foreach (string file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string relativePath = Path.GetRelativePath(rootDirectory, file);
            string? dir = Path.GetDirectoryName(relativePath);
            string name = Path.GetFileName(relativePath);

            string dirPart = string.IsNullOrEmpty(dir)
                ? string.Empty
                : Markup.Escape(dir + Path.DirectorySeparatorChar);

            int lineCount = File.ReadLines(file).Count();
            total += lineCount;

            table.AddRow(
                $"[grey]{dirPart}[/][white]{Markup.Escape(name)}[/]",
                $"[blue]{lineCount:N0}[/]"
            );
        }

        table.Columns[0].Footer = new Markup($"[bold]Total[/] [grey]({files.Length} files)[/]");
        table.Columns[1].Footer = new Markup($"[bold green]{total:N0}[/]");

        AnsiConsole.Write(table);

        return (files.Length, total);
    }
}