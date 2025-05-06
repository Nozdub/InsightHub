namespace InsightHub.ArticleFetcher.Models
{
    public class DetailedArticleDto
    {

        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string Doi { get; set; }
        public string PdfUrl { get; set; }
        public string LandingPageUrl { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public string HostType { get; set; }
        public string JournalName { get; set; }

    }
}
