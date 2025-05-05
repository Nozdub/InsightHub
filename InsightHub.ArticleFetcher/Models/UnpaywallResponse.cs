using System.Text.Json.Serialization;

namespace InsightHub.ArticleFetcher.Models
{
    public class UnpaywallResponse
    {

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("doi")]
        public string Doi { get; set; }

        [JsonPropertyName("doi_url")]
        public string LandingPageUrl { get; set; }

        [JsonPropertyName("z_authors")]
        public List<Author> Authors { get; set; }

        [JsonPropertyName("best_oa_location")]
        public BestOALocation BestOALocation { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("given")]
        public string Given { get; set; }

        [JsonPropertyName("family")]
        public string Family { get; set; }
    }

    public class BestOALocation
    {
        [JsonPropertyName("url_for_pdf")]
        public string UrlForPdf { get; set; }

    }
}
