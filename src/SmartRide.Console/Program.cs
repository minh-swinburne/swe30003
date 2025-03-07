namespace SmartRide.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
        Console.WriteLine(AppContext.BaseDirectory);
        Console.WriteLine(solutionRoot);
        Console.WriteLine(Directory.GetFiles(solutionRoot));
    }
}
