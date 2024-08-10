using Microsoft.Extensions.Configuration;
using OpenAI.Batch;
using System.ClientModel;

namespace OpenApiBatchExample;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Read API Key from User Secrets (Visual Studio: right-click on project -> Manage User Secrets)
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var apiKey = configuration["OpenAiDeveloperKey"] ?? throw new ArgumentException("OpenAPI Key is missing in User Secrets configuration");

        // Create a client for the OpenAI Batch API
        BatchClient batchClient = new(new ApiKeyCredential(apiKey));

        // Create a batch request
        // `OpenAI` library doesn't deliver a strongly-typed model (yet?),
        // so property names come directly from docs at/ https://platform.openai.com/docs/api-reference/batch/create
        var batchRequest = new
        {
            // file with requests was uploaded manually at https://platform.openai.com/storage/files
            // example of file content: https://platform.openai.com/docs/guides/batch/1-preparing-your-batch-file
            input_file_id = "file-f8fB0yH04nq9QmiWYYBtyYKC",
            endpoint = "/v1/chat/completions",
            completion_window = "24h", // currently only 24h is supported
            metadata = new
            {
                // available later via Batch API, although not exposed in the UI, so usefulness is questionable
                description = "C# eval job test"
            }
        };

        var response = await batchClient.CreateBatchAsync(BinaryContent.Create(BinaryData.FromObjectAsJson(batchRequest)));
        var rawResponse = response.GetRawResponse();

        Console.WriteLine(rawResponse.Content);

        // this only writes METADATA about the batch job to the output; the job might take up to 24h to complete
        // Metadata is something like:
        //{
        //    "id": "batch_lF3mjwKIDUusSrf43nCnkqOz",
        //    "object": "batch",
        //    "endpoint": "/v1/chat/completions",
        //    "input_file_id": "file-f8fB0yH04nq9QmiWYYBtyYKC",
        //    "status": "validating",
        //    "created_at": 1722955749,
        //    "expires_at": 1723042149,
        //    (...)
        //    "metadata": {
        //        "description": "C# eval job test"
        //    }
        //}

    }

}
