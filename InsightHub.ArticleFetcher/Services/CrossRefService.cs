using InsightHub.ArticleFetcher.Models;
using System.Text.Json;


namespace InsightHub.ArticleFetcher.Services
{
    public class CrossRefService
    {
        private readonly HttpClient _httpClient;

        public CrossRefService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CrossRefDto>> SearchDoisAsync(string query)
        {
            var url = $"https://api.crossref.org/works?query={Uri.EscapeDataString(query)}&rows=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<CrossRefDto>();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("=== Raw CrossRef JSON (SearchDoisAsync) ===");
            Console.WriteLine(content);
            Console.WriteLine("===========================================");

            var data = JsonSerializer.Deserialize<CrossRefResponse>(content);

            if (data?.message?.items == null)
                return new List<CrossRefDto>();

            return data.message.items
                .Select(i => new CrossRefDto
                {
                    Doi = i.DOI,
                    Title = i.title?.FirstOrDefault() ?? "Untitled",
                    Authors = i.author?.Select(a => $"{a.given} {a.family}").ToList(),
                    Year = i.published?.dateParts?.FirstOrDefault()?.FirstOrDefault() ?? 0
                })
                .ToList();
            }

        public async Task<List<CrossRefDto>> SearchAndEnrichWithUnpaywallAsync(string query, int page, int pageSize)
        {
            int offset = (page - 1) * pageSize;
            var url = $"https://api.crossref.org/works?query={Uri.EscapeDataString(query)}&rows={pageSize}&offset={offset}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<CrossRefDto>();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("=== Raw CrossRef JSON ===");
            Console.WriteLine(content);
            Console.WriteLine("=========================");

            var data = JsonSerializer.Deserialize<CrossRefResponse>(content);

            if (data?.message?.items == null)
                return new List<CrossRefDto>();

            var results = new List<CrossRefDto>();

            foreach (var item in data.message.items)
            {
                var dto = new CrossRefDto
                {
                    Doi = item.DOI,
                    Title = item.title?.FirstOrDefault() ?? "Untitled",
                    Authors = item.author?.Select(a => $"{a.given} {a.family}").ToList(),
                    Year = item.published?.dateParts?.FirstOrDefault()?.FirstOrDefault() ?? 0
                };

                //  Log missing metadata (like null year or authors)
                if (dto.Year == null || dto.Authors == null || !dto.Authors.Any())
                {
                    Console.WriteLine($"[INFO] DOI {dto.Doi} has missing data: " +
                        $"{(dto.Year == null ? "Year " : "")}" +
                        $"{(dto.Authors == null || !dto.Authors.Any() ? "Authors" : "")}");
                }

                if (!string.IsNullOrWhiteSpace(dto.Doi))
                {
                    var unpaywallUrl = $"https://api.unpaywall.org/v2/{dto.Doi}?email=mytrados@gmail.com";
                    var unpaywallResp = await _httpClient.GetAsync(unpaywallUrl);

                    if (unpaywallResp.IsSuccessStatusCode)
                    {
                        var unpaywallContent = await unpaywallResp.Content.ReadAsStringAsync();
                        Console.WriteLine($"=== Unpaywall JSON for DOI {dto.Doi} ===");
                        Console.WriteLine(unpaywallContent);
                        Console.WriteLine("========================================");
                        var unpaywallData = JsonSerializer.Deserialize<UnpaywallResponse>(unpaywallContent);

                        if (unpaywallData?.BestOALocation != null)
                        {
                            dto.PdfUrl = unpaywallData.BestOALocation.UrlForPdf;
                            dto.LandingPageUrl = unpaywallData.BestOALocation.UrlForLandingPage;
                            dto.License = unpaywallData.BestOALocation.License;
                        }
                    }
                }

                results.Add(dto);
            }

            return results;
        }




    }
}
