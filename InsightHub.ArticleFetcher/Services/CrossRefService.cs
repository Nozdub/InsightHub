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

        public async Task<List<CrossRefDto>> SearchAndEnrichWithUnpaywallAsync(string query)
        {
            var url = $"https://api.crossref.org/works?query={Uri.EscapeDataString(query)}&rows=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<CrossRefDto>();

            var content = await response.Content.ReadAsStringAsync();
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

                // Fetch Unpaywall data
                if (!string.IsNullOrWhiteSpace(dto.Doi))
                {
                    var unpaywallUrl = $"https://api.unpaywall.org/v2/{dto.Doi}?email=mytrados@gmail.com";
                    var unpaywallResp = await _httpClient.GetAsync(unpaywallUrl);

                    if (unpaywallResp.IsSuccessStatusCode)
                    {
                        var unpaywallContent = await unpaywallResp.Content.ReadAsStringAsync();
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
