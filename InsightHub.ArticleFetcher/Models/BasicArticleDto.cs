namespace InsightHub.ArticleFetcher.Models
{
    public class BasicArticleDto
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }

        public string? LandingPageUrl { get; set; }



    }
}
