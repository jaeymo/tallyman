using tallyman.Cli;

namespace tallyman.Tests;

public class BasicUnitTest
{
    [Fact]
    public void ExecutesSuccessfully()
    {
        string directory = Directory.CreateTempSubdirectory("tallyman-test-").FullName;

        try
        {
            File.WriteAllLines(Path.Combine(directory, "first.txt"),
            [
                "Hello, world!",
                "This is the first testing file."
            ]
            );

            File.WriteAllLines(
                Path.Combine(directory, "second.txt"),
                [
                    "Another file.",
                    "With some random text.",
                    "Testing the CLI.",
                    "This should be counted."
                ]
            );

            int result = Program.Main([directory]);

            Assert.Equal(0, result);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}