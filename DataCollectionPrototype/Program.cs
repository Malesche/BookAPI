using DataCollectionPrototype.Consolidation;
using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.OpenLibrary;
using DataCollectionPrototype.TargetWriting;

namespace DataCollectionPrototype
{
    //public class Program
    //{
    //    public static async Task Main(string[] args)
    //    {
    //        using HttpClient client = new();
    //        client.DefaultRequestHeaders.Accept.Clear();
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

    //        await ProcessWorkAsync(client);
    //        await ProcessBookAsync(client);

    //        var openLibClient = new OpenLibraryClient();
    //        OpenLibraryBook book = await openLibClient.GetOpenLibraryBookAsync("books/OL8364844M");

    //        Console.ReadKey();
    //    }

    //    static async Task ProcessWorkAsync(HttpClient client)
    //    {
    //        await using Stream stream = await client.GetStreamAsync("https://openlibrary.org/works/OL262758W");
    //        var work = await JsonSerializer.DeserializeAsync<OpenLibraryWork>(stream);

    //        Console.WriteLine(work.title);
    //        Console.WriteLine(work.key);
    //        foreach (var author in work.authors)
    //        { 
    //            Console.WriteLine(author.author.key);
    //        }
    //        //Console.WriteLine(work.Title);
    //        //Console.WriteLine(work.PubDate);
    //        //Console.WriteLine(work.OpenLibraryWorkPath);
    //    }

    //    static async Task ProcessBookAsync(HttpClient client)
    //    {
    //        await using Stream stream = await client.GetStreamAsync("https://openlibrary.org/books/OL8364844M");
    //        var book = await JsonSerializer.DeserializeAsync<OpenLibraryBook>(stream);

    //        Console.WriteLine(book.title);
    //        Console.WriteLine(book.key);
    //        foreach (var author in book.authors)
    //        {
    //            Console.WriteLine(author.key);
    //        }

    //        foreach (var contribution in book.contributions)
    //        {
    //            Console.WriteLine(contribution);
    //        }
    //        //Console.WriteLine(work.Title);
    //        //Console.WriteLine(work.PubDate);
    //        //Console.WriteLine(work.OpenLibraryWorkPath);
    //    }
    //}

    public static class Program
    {
        public static async Task Main()
        {
            try
            {
                var runner = new DataCollectionRunner(
                    new []
                    {
                        new OpenLibraryGatherer()
                    },
                    new PassThroughConsolidator(),
                    new LibraryApiWriter(),
                    new RunnerConfiguration()
                    );

                await runner.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    
}

//using System.Net.Http.Headers;

//namespace DataCollectionPrototype
//{
//    public class Program
//    {
//        public static void Main(string[] args)
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
//    }
//}