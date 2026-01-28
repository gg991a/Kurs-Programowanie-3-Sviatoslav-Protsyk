using RestSharp;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string baseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7157/";
var client = new RestClient(baseUrl);

Console.WriteLine("--- SPORTS STORE CLIENT ---");
var request = new RestRequest("api/Products", Method.Get);
var response = await client.ExecuteAsync(request);

if (response.IsSuccessful && response.Content != null)
{
    var options = new JsonSerializerOptions { WriteIndented = true };
    var json = JsonSerializer.Deserialize<JsonElement>(response.Content);
    Console.WriteLine("Lista produktów:\n" + JsonSerializer.Serialize(json, options));
}

Console.WriteLine("\nNaciśnij klawisz...");
Console.ReadKey();