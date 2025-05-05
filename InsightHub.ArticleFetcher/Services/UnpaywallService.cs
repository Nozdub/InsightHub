using System.Net.Http;
using System.Threading.Tasks;

namespace InsightHub.ArticleFetcher.Services
{
    
    public class UnpaywallService
    {
        private readonly HttpClient _httpClient;

        public UnpaywallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRawArticlesAsync(string query)
        {
            // TEMP: Using a real DOI
            var doi = "10.1093/biosci/biz088";
            var url = $"https://api.unpaywall.org/v2/{doi}?email=mytrados@gmail.com";

            // log outgoing request
            Console.WriteLine($"Sending GET request to: {url}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed with status: {response.StatusCode}");
                return $"Error: {response.StatusCode}";
            }

            // Log the raw JSON response
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response received:");
            Console.WriteLine(content);

            return content;
        }


    }
}