using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using InsightHub.ArticleFetcher.Models;

namespace InsightHub.ArticleFetcher.Services
{
    
    public class UnpaywallService
    {
        private readonly HttpClient _httpClient;

        public UnpaywallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ArticleDto> GetRawArticlesAsync(string query)
        {
            var doi = "10.1093/biosci/biz088";
            var url = $"https://api.unpaywall.org/v2/{doi}?email=mytrados@gmail.com";


            Console.WriteLine($"Sending GET request to: {url}");
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed with status: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response received");

            var article = JsonSerializer.Deserialize<UnpaywallResponse>(content);

            if (article == null)
                return null;

            var dto = new ArticleDto
            {
                Title = article.Title,
                Year = article.Year,
                Publisher = article.Publisher,
                Doi = article.Doi,
                LandingPageUrl = article.LandingPageUrl,
                PdfUrl = article.BestOALocation?.UrlForPdf,
                Authors = article.Authors?.Select(a => $"{a.Given} {a.Family}").ToList()
            };

            return dto;
        }

        public async Task<DetailedArticleDto> GetArticleByDoiAsync(string doi)
        {

            var url = $"https://api.unpaywall.org/v2/{doi}?email=mytrados@gmail.com";
            Console.WriteLine($"Fetching detailed article for DOI: {doi}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var article = JsonSerializer.Deserialize<UnpaywallResponse>(content);

            if (article == null)
                return null;

            var dto = new DetailedArticleDto
            {
                Title = article.Title,
                Year = article.Year,
                Publisher = article.Publisher,
                Doi = article.Doi,
                JournalName = article.JournalName,
                LandingPageUrl = article.LandingPageUrl,
                PdfUrl = article.BestOALocation?.UrlForPdf,
                Version = article.BestOALocation?.Version,
                License = article.BestOALocation?.License,
                HostType = article.BestOALocation?.HostType,
                Authors = article.Authors?.Select(a => $"{a.Given} {a.Family}").ToList()

            };

            return dto;
        }

    }
}