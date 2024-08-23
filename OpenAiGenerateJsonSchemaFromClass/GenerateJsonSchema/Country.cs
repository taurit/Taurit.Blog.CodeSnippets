using System.ComponentModel;

namespace GenerateJsonSchema;

// Declare the format of the GPT model's response as a C# class
public class Country
{
    [Description("Country name preferred in English language")]
    public string InternationalName { get; set; }

    public int EstimatedPopulation { get; set; }

    [Description("Names of 3 people from that country most recognizable abroad")]
    public List<string> FamousPeopleExample { get; set; }
}