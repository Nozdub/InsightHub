using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InsightHub.ArticleFetcher.Models
{
    public class SemanticScholarResponse
    {
        [JsonPropertyName("data")]
        public List<Paper> Data { get; set; }
    }

    public class Paper
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("authors")]
        public List<Author> Authors { get; set; }

        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonPropertyName("venue")]
        public string Venue { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
