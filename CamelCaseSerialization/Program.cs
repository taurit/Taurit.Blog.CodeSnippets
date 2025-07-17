namespace CamelCaseSerialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class TestObject
{
    public string URL { get; set; } = "https://example.com";
    public string URLPath { get; set; } = "/api/v1";
    public string MyURL { get; set; } = "https://mysite.com";
    public string IOStream { get; set; } = "FileStream";
    public string M4AFileName { get; set; } = "audio.m4a";
}

class Program
{
    static void Main()
    {
        var testObject = new TestObject();
        var results = new Dictionary<string, Dictionary<string, string>>();
        var propertyNames = typeof(TestObject).GetProperties().Select(p => p.Name).ToList();

        // Initialize results dictionary
        foreach (var prop in propertyNames)
        {
            results[prop] = new Dictionary<string, string>();
        }

        // Newtonsoft.Json (Default)
        var defaultJson = JsonConvert.SerializeObject(testObject);
        var defaultParsed = JObject.Parse(defaultJson);
        foreach (var prop in defaultParsed.Properties())
        {
            results[prop.Name]["Newtonsoft.Json (Default)"] = prop.Name;
        }

        // Newtonsoft.Json (CamelCase)
        var camelCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };
        var camelCaseJson = JsonConvert.SerializeObject(testObject, camelCaseSettings);
        var camelCaseParsed = JObject.Parse(camelCaseJson);
        foreach (var prop in propertyNames)
        {
            var serializedProp = camelCaseParsed.Properties().FirstOrDefault(p => p.Value.ToString() == testObject.GetType().GetProperty(prop).GetValue(testObject).ToString());
            results[prop]["Newtonsoft.Json (CamelCase)"] = serializedProp?.Name ?? "N/A";
        }

        // Newtonsoft.Json (SnakeCase)
        var snakeCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        };
        var snakeCaseJson = JsonConvert.SerializeObject(testObject, snakeCaseSettings);
        var snakeCaseParsed = JObject.Parse(snakeCaseJson);
        foreach (var prop in propertyNames)
        {
            var serializedProp = snakeCaseParsed.Properties().FirstOrDefault(p => p.Value.ToString() == testObject.GetType().GetProperty(prop).GetValue(testObject).ToString());
            results[prop]["Newtonsoft.Json (SnakeCase)"] = serializedProp?.Name ?? "N/A";
        }

        // Newtonsoft.Json (KebabCase)
        var kebabCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new KebabCaseNamingStrategy() }
        };
        var kebabCaseJson = JsonConvert.SerializeObject(testObject, kebabCaseSettings);
        var kebabCaseParsed = JObject.Parse(kebabCaseJson);
        foreach (var prop in propertyNames)
        {
            var serializedProp = kebabCaseParsed.Properties().FirstOrDefault(p => p.Value.ToString() == testObject.GetType().GetProperty(prop).GetValue(testObject).ToString());
            results[prop]["Newtonsoft.Json (KebabCase)"] = serializedProp?.Name ?? "N/A";
        }

        // System.Text.Json (Default)
        var systemDefaultJson = System.Text.Json.JsonSerializer.Serialize(testObject);
        var systemDefaultDoc = JsonDocument.Parse(systemDefaultJson);
        foreach (var prop in systemDefaultDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[originalProp]["System.Text.Json (Default)"] = prop.Name;
        }

        // System.Text.Json (CamelCase)
        var systemCamelCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var systemCamelCaseJson = System.Text.Json.JsonSerializer.Serialize(testObject, systemCamelCaseOptions);
        var systemCamelCaseDoc = JsonDocument.Parse(systemCamelCaseJson);
        foreach (var prop in systemCamelCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[originalProp]["System.Text.Json (CamelCase)"] = prop.Name;
        }

        // System.Text.Json (SnakeCaseLower)
        var systemSnakeCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        var systemSnakeCaseJson = System.Text.Json.JsonSerializer.Serialize(testObject, systemSnakeCaseOptions);
        var systemSnakeCaseDoc = JsonDocument.Parse(systemSnakeCaseJson);
        foreach (var prop in systemSnakeCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[originalProp]["System.Text.Json (SnakeCaseLower)"] = prop.Name;
        }

        // System.Text.Json (KebabCaseLower)
        var systemKebabCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };
        var systemKebabCaseJson = System.Text.Json.JsonSerializer.Serialize(testObject, systemKebabCaseOptions);
        var systemKebabCaseDoc = JsonDocument.Parse(systemKebabCaseJson);
        foreach (var prop in systemKebabCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[originalProp]["System.Text.Json (KebabCaseLower)"] = prop.Name;
        }

        // Output markdown table
        Console.WriteLine("| Property Name | Newtonsoft.Json (Default) | Newtonsoft.Json (CamelCase) | Newtonsoft.Json (SnakeCase) | Newtonsoft.Json (KebabCase) | System.Text.Json (Default) | System.Text.Json (CamelCase) | System.Text.Json (SnakeCaseLower) | System.Text.Json (KebabCaseLower) |");
        Console.WriteLine("|---------------|---------------------------|------------------------------|------------------------------|------------------------------|----------------------------|-------------------------------|-----------------------------------|-----------------------------------|");

        foreach (var prop in propertyNames)
        {
            Console.Write($"| `{prop}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("Newtonsoft.Json (Default)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("Newtonsoft.Json (CamelCase)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("Newtonsoft.Json (SnakeCase)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("Newtonsoft.Json (KebabCase)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("System.Text.Json (Default)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("System.Text.Json (CamelCase)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("System.Text.Json (SnakeCaseLower)", "N/A")}` |");
            Console.Write($" `{results[prop].GetValueOrDefault("System.Text.Json (KebabCaseLower)", "N/A")}` |");
            Console.WriteLine();
        }
    }
}