using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollectionPrototype.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace DataCollectionPrototype.Clients
{
    public class OpenLibraryClient
    {
        //private string basePath = "https://openlibrary.org/";

        public async Task<OpenLibraryBook> GetOpenLibraryBookAsync(string bookKey)
        {   
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://openlibrary.org/");
            OpenLibraryBook book = null;

            HttpResponseMessage response = await client.GetAsync(bookKey);
            if (response.IsSuccessStatusCode)
            {
                book = await response.Content.ReadAsAsync<OpenLibraryBook>();
            }

            return book;
        }

        protected void PrintBook(OpenLibraryBook book)
        {
            Console.WriteLine(book.title);
            Console.WriteLine(book.key);
            foreach (var author in book.authors)
            {
                Console.WriteLine(author.key);
            }

            foreach (var contribution in book.contributions)
            {
                Console.WriteLine(contribution);
            }
        }


    }
}
