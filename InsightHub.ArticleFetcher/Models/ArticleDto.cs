namespace InsightHub.ArticleFetcher.Models
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string Doi { get; set; }
        public string PdfUrl { get; set; }
        public string LandingPageUrl { get; set; }


    }
}
