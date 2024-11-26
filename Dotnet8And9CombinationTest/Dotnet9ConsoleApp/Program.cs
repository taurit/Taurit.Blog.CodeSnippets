namespace Dotnet9ConsoleApp;

internal static class Program
{
    private static void Main()
    {
        try
        {
            Console.WriteLine("# Calling a compatible method");
            Console.WriteLine("Dotnet8Library.Dotnet8Helper.Add(1, 2): " + Dotnet8Library.Dotnet8Helper.Add(1, 2));
            Console.WriteLine("");

            Console.WriteLine("# Calling an incompatible method");
            Dotnet8Library.Dotnet8Helper.UseBinaryFormatter();

            Console.WriteLine("Done (no exceptions observed)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}
