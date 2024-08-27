using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using Spectre.Console;
using Spectre.Console.Json;

namespace GenerateJsonSchema;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Prepare the JSON schema
        JSchemaGenerator generator = new JSchemaGenerator();
        generator.DefaultRequired = Required.Always; // required by OpenAI

        JSchema schema = generator.Generate(typeof(Country));
        schema.AllowAdditionalProperties = false; // required by OpenAI

        var schemaAsString = schema.ToString();
        AnsiConsole.Write(new JsonText(schemaAsString));

        // Send query to OpenAI
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var openAiDeveloperKey = config["OpenAiDeveloperKey"]!;
        var openAiOrganizationId = config["OpenAiOrganization"]!;

        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = openAiDeveloperKey,
            Organization = openAiOrganizationId,
            DefaultModelId = "gpt-4o-2024-08-06"
        });

        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant."),
                ChatMessage.FromUser("Provide information about Poland"),
            },
            ResponseFormat = new ResponseFormat()
            {
                Type = StaticValues.CompletionStatics.ResponseFormat.JsonSchema,
                JsonSchema = schema
            }
        });

        if (completionResult.Successful)
        {
            var responseText = completionResult.Choices.First().Message.Content;
            AnsiConsole.Write(new JsonText(responseText));
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {completionResult.Error?.Message}");
        }


    }
}
