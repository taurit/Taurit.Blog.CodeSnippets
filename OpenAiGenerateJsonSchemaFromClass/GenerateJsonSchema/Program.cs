using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Chat;
using Spectre.Console;
using Spectre.Console.Json;
using System.ClientModel;

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
        AnsiConsole.MarkupLine("[yellow on blue1]Schema:[/]");
        AnsiConsole.Write(new JsonText(schemaAsString));
        AnsiConsole.WriteLine();

        // Send query to OpenAI
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var openAiDeveloperKey = config["OpenAiDeveloperKey"]!;
        var openAiOrganizationId = config["OpenAiOrganization"]!;

        var openAiClientOptions = new OpenAIClientOptions { OrganizationId = openAiOrganizationId };
        ChatClient client = new(model: "gpt-4o-2024-08-06", new ApiKeyCredential(openAiDeveloperKey), openAiClientOptions);

        var options = new ChatCompletionOptions
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat("countries_schema", new BinaryData(schemaAsString), null, true)

        };
        List<ChatMessage> messages =
        [
            new SystemChatMessage("You are a helpful assistant."),
            new UserChatMessage("Provide information about Poland")
        ];

        ChatCompletion completion = await client.CompleteChatAsync(messages, options);
        var responseToPrompt = completion.Content[0].Text;

        AnsiConsole.MarkupLine("[yellow on blue1]ChatGPT response:[/]");
        AnsiConsole.Write(new JsonText(responseToPrompt));
    }
}
