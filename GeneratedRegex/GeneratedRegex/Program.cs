using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.RegularExpressions;

var summary = BenchmarkRunner.Run<MyBenchmark>();
Console.WriteLine(summary);

public partial class MyBenchmark
{
    private const string SampleInput = "   input  with     too many whitespaces   ";

    [Benchmark]
    public string OldImplementation() => OldImplementation(SampleInput);

    [Benchmark]
    public string NewImplementation() => NewImplementation(SampleInput);

    // OLD IMPLEMENTATION
    private static readonly Regex MultipleWhitespacesRegex = new(@"\s+", RegexOptions.Compiled);
    private string OldImplementation(string input) => MultipleWhitespacesRegex.Replace(input, " ");

    // NEW IMPLEMENTATION
    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleWhitespacesGeneratedRegex();
    private string NewImplementation(string input) => MultipleWhitespacesGeneratedRegex().Replace(input, " ");
}