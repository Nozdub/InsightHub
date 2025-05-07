using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InsightHub.ArticleFetcher.Services
{
    public class LocalArticleIndex
    {

        // Simulating a small local database of articles 
        private static readonly List<ArticleStub> _articles = new List<ArticleStub>
        {
            new ArticleStub
            {
                Title = "World Scientists' Warning of a Climate Emergency",
                Topic = "climate change", 
                Doi = "10.1093/biosci/biz088"

            },
            new ArticleStub
            {
                Title = "Deep Learning for Medical Image Analysis",
                Topic = "artificial intelligence",
                Doi = "10.1016/j.media.2017.07.005"
            },
            new ArticleStub
            {
                Title = "Quantum Supremacy Using a Programmable Superconducting Processor",
                Topic = "quantum computing",
                Doi = "10.1038/s41586-019-1666-5"
            },
            new ArticleStub
            {
                Title = "The CRISPR-Cas9 system for genome editing",
                Topic = "genetics",
                Doi = "10.1126/science.1231143"
            },
            new ArticleStub
            {
                Title = "A Global Overview of Carbon Budgeting",
                Topic = "environment",
                Doi = "10.5194/essd-12-3269-2020"
            }
        };

        public List<string> SearchDois(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();  

            query = query.ToLower();

            return _articles
                .Where(a => a.Title.ToLower().Contains(query) || a.Topic.ToLower().Contains(query))
                .Select(a => a.Doi)
                .ToList();
        }

        private class ArticleStub
        {
            public string Title { get; set; }
            public string Topic { get; set; }
            public string Doi { get; set; } 

        }
    }
}
