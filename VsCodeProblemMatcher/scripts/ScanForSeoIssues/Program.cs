namespace ScanForSeoIssues;

internal class Program
{
  static int Main(string[] args)
  {
    // emit some warnings
    var filePath = "D:\\Projekty\\FlashcardSpace.Blog\\flashcard-space\\.vscode\\tasks.json";

    Console.WriteLine($"warning: file '{filePath}', line '15', column '17': custom warning message");
    Console.WriteLine($"error: file '{filePath}', line '16', column '18': custom error message");

    // return non-zero error code to signal that check failed and stop the build
    return 1;
  }
}
