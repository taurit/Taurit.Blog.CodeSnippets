using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        var serializers = new List<string>();

        // Newtonsoft.Json (Default)
        var serializerName = "Newtonsoft.Json (Default)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var defaultJson = JsonConvert.SerializeObject(testObject);
        var defaultParsed = JObject.Parse(defaultJson);
        foreach (var prop in defaultParsed.Properties())
        {
            var originalProp = propertyNames.FirstOrDefault(p => testObject.GetType().GetProperty(p).GetValue(testObject).ToString() == prop.Value.ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // Newtonsoft.Json (CamelCase)
        serializerName = "Newtonsoft.Json (CamelCase)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var camelCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };
        var camelCaseJson = JsonConvert.SerializeObject(testObject, camelCaseSettings);
        var camelCaseParsed = JObject.Parse(camelCaseJson);
        foreach (var prop in camelCaseParsed.Properties())
        {
            var originalProp = propertyNames.FirstOrDefault(p => testObject.GetType().GetProperty(p).GetValue(testObject).ToString() == prop.Value.ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // Newtonsoft.Json (SnakeCase)
        serializerName = "Newtonsoft.Json (SnakeCase)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var snakeCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        };
        var snakeCaseJson = JsonConvert.SerializeObject(testObject, snakeCaseSettings);
        var snakeCaseParsed = JObject.Parse(snakeCaseJson);
        foreach (var prop in snakeCaseParsed.Properties())
        {
            var originalProp = propertyNames.FirstOrDefault(p => testObject.GetType().GetProperty(p).GetValue(testObject).ToString() == prop.Value.ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // Newtonsoft.Json (KebabCase)
        serializerName = "Newtonsoft.Json (KebabCase)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var kebabCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new KebabCaseNamingStrategy() }
        };
        var kebabCaseJson = JsonConvert.SerializeObject(testObject, kebabCaseSettings);
        var kebabCaseParsed = JObject.Parse(kebabCaseJson);
        foreach (var prop in kebabCaseParsed.Properties())
        {
            var originalProp = propertyNames.FirstOrDefault(p => testObject.GetType().GetProperty(p).GetValue(testObject).ToString() == prop.Value.ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // System.Text.Json (Default)
        serializerName = "System.Text.Json (Default)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var systemDefaultJson = JsonSerializer.Serialize(testObject);
        var systemDefaultDoc = JsonDocument.Parse(systemDefaultJson);
        foreach (var prop in systemDefaultDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // System.Text.Json (CamelCase)
        serializerName = "System.Text.Json (CamelCase)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var systemCamelCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var systemCamelCaseJson = JsonSerializer.Serialize(testObject, systemCamelCaseOptions);
        var systemCamelCaseDoc = JsonDocument.Parse(systemCamelCaseJson);
        foreach (var prop in systemCamelCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // System.Text.Json (SnakeCaseLower)
        serializerName = "System.Text.Json (SnakeCaseLower)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var systemSnakeCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        var systemSnakeCaseJson = JsonSerializer.Serialize(testObject, systemSnakeCaseOptions);
        var systemSnakeCaseDoc = JsonDocument.Parse(systemSnakeCaseJson);
        foreach (var prop in systemSnakeCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // System.Text.Json (KebabCaseLower)
        serializerName = "System.Text.Json (KebabCaseLower)";
        serializers.Add(serializerName);
        results[serializerName] = new Dictionary<string, string>();
        var systemKebabCaseOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };
        var systemKebabCaseJson = JsonSerializer.Serialize(testObject, systemKebabCaseOptions);
        var systemKebabCaseDoc = JsonDocument.Parse(systemKebabCaseJson);
        foreach (var prop in systemKebabCaseDoc.RootElement.EnumerateObject())
        {
            var originalProp = propertyNames.FirstOrDefault(p => prop.Value.GetString() == testObject.GetType().GetProperty(p).GetValue(testObject).ToString());
            if (originalProp != null)
                results[serializerName][originalProp] = prop.Name;
        }

        // Output markdown table
        Console.WriteLine("The following table shows how different JSON serializers handle common acronym properties:");

        // Header row
        Console.Write("| Serializer |");
        foreach (var prop in propertyNames)
        {
            Console.Write($" `{prop}` |");
        }
        Console.WriteLine();

        // Separator row
        Console.Write("| --------------------------------- |");
        foreach (var prop in propertyNames)
        {
            Console.Write(" ----------- |");
        }
        Console.WriteLine();

        // Data rows
        foreach (var serializer in serializers)
        {
            Console.Write($"| {serializer} |");
            foreach (var prop in propertyNames)
            {
                var value = results[serializer].GetValueOrDefault(prop, "N/A");
                Console.Write($" `{value}` |");
            }
            Console.WriteLine();
        }
    }
}