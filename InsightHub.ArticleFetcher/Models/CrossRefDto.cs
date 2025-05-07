namespace InsightHub.ArticleFetcher.Models
{
    public class CrossRefDto
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public int Year { get; set; }
        public string Doi { get; set; }
    }
}

