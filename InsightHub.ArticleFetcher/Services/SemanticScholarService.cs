using InsightHub.ArticleFetcher.Models;
using System.Text.Json;

namespace InsightHub.ArticleFetcher.Services
{
    public class SemanticScholarService
    {
        private readonly HttpClient _httpClient;

        public SemanticScholarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BasicArticleDto>> SearchAsync(string query)
        {
            var requestUrl = $"https://api.semanticscholar.org/graph/v1/paper/search?query={query}&limit=50&fields=title,authors,year,venue,url";


            var response = await _httpClient.GetAsync(requestUrl);

            // Handle 429 Too Many Requests with Retry-After support
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                int retryAfterSeconds = 3;

                if (response.Headers.TryGetValues("Retry-After", out var values))
                {
                    var headerValue = values.FirstOrDefault();
                    if (int.TryParse(headerValue, out int parsedRetry))
                    {
                        retryAfterSeconds = parsedRetry;
                    }
                }

                Console.WriteLine($"Rate limited. Waiting {retryAfterSeconds} seconds before retry...");
                await Task.Delay(retryAfterSeconds * 1000);
                response = await _httpClient.GetAsync(requestUrl);
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Semantic Scholar API call failed: {response.StatusCode}");
                return new List<BasicArticleDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<SemanticScholarResponse>(content);

            if (data?.Data == null)
            {
                Console.WriteLine("=== Raw Semantic Scholar JSON ===");
                Console.WriteLine(content);
                Console.WriteLine("=================================");
                return new List<BasicArticleDto>();
            }

            return data.Data
                .Select(paper => new BasicArticleDto
                {
                    Title = paper.Title ?? "Untitled",
                    Authors = paper.Authors?.Select(a => a.Name).ToList() ?? new List<string>(),
                    Year = paper.Year ?? 0,
                    Publisher = paper.Venue ?? "",
                    LandingPageUrl = paper.Url
                })
                .ToList();
        }
    }
}
