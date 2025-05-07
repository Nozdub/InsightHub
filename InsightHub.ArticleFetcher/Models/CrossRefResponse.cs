using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InsightHub.ArticleFetcher.Models
{
    public class CrossRefResponse
    {
        public Message message { get; set; }

        public class Message
        {
            public List<Item> items { get; set; }
        }

        public class Item
        {
            public string DOI { get; set; }
            public List<string> title { get; set; }
            public List<Author> author { get; set; }
            public Published published { get; set; }
        }

        public class Author
        {
            public string given { get; set; }
            public string family { get; set; }
        }

        public class Published
        {
            [JsonPropertyName("date-parts")]
            public List<List<int>> dateParts { get; set; }
        }
    }
}
