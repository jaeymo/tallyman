using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

public class CountSettings : CommandSettings
{
    [CommandArgument(0, "<directory>")]
    [Description("The path to the target directory")]
    public string Directory { get; init; } = string.Empty;
}

public class CountCommand : Command<CountSettings>
{
    protected override int Execute(
        CommandContext context,
        CountSettings settings,
        CancellationToken cancellationToken)
    {
        if (!System.IO.Directory.Exists(settings.Directory))
        {
            AnsiConsole.MarkupLine(
                $"[red]✗ Directory not found:[/] [yellow]{Markup.Escape(settings.Directory)}[/]"
            );
            return 1;
        }

        string rootDirectory = Path.GetFullPath(settings.Directory);

        string[] files = System.IO.Directory
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
            return 0;
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
        AnsiConsole.MarkupLine("[green]✓ Done[/]");

        return 0;
    }
}