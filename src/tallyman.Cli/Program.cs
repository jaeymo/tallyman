using Spectre.Console.Cli;

namespace tallyman.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        return new CommandApp<CountCommand>().Run(args);
    }
}