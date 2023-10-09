//using DataCollectionPrototype;
//using System.Net.Http.Headers;
//using System.Net.Json;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);


//    }
//}



//using System.Net.Http.Headers;
//using System.Text.Json;
//using DataCollectionPrototype;

//using HttpClient client = new();
//client.DefaultRequestHeaders.Accept.Clear();
//client.DefaultRequestHeaders.Accept.Add(
//    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
//client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

//await ProcessRepositoriesAsync(client);

//static async Task ProcessRepositoriesAsync(HttpClient client)
//{
//    await using Stream stream = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
//    var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(stream);

//    foreach (var repo in repositories ?? Enumerable.Empty<Repository>())
//    {
//        Console.WriteLine(repo.Name);   
//    }
//}













//static async Task Main(string[] args)
//        {
//            using var client = new HttpClient();
//            client.BaseAddress = new Uri("https://openlibrary.org/");
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            HttpResponseMessage response = client.GetAsync("works/OL45804W.json").Result;
//            if (response.IsSuccessStatusCode)
//            {
//                var responseContent = response.Content.ReadAsStringAsync().Result;
//                Console.WriteLine(responseContent);
//            }
//            else
//            {
//                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
//            }
//        }

//namespace DataCollectionPrototype
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {

//        }
//    }
//}
