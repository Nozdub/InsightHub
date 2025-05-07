using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsightHub.ArticleFetcher.Services;


namespace InsightHub.Tests
{
    public class LocalArticleIndexTests
    {

        [Fact]
        public void SearchDois_ReturnsMatchingDoi_WhenKeywordMatchesTitleOrTopic()
        {

            // Arrange
            var index = new LocalArticleIndex();

            // Act
            var results = index.SearchDois("climate");


            // Assert
            Assert.NotNull(results);
            Assert.Contains("10.1093/biosci/biz088", results);
        }

        [Fact]
        public void SearchDois_ReturnsEmptyList_WhenNoMatchFound()
        {
            // Arrange
            var index = new LocalArticleIndex();

            // Act
            var results = index.SearchDois("dinosaurs");

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }
    }
}
