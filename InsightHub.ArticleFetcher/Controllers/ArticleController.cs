using InsightHub.ArticleFetcher.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsightHub.ArticleFetcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        [HttpGet("search")]
        public ActionResult<List<ArticleDto>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            // For testing purpose, return heardcoded list.
            var results = new List<ArticleDto>
            {
                new ArticleDto
                {
                    Title = "Cilmate Change and teh Arctic",
                    Author = "Dr. Jane Smith",
                    Year = 2023,
                    Publisher = "Nature",
                    Doi = "10.1234/exampledoi1",
                    Url = "https://example.com/article1"
                },
                new ArticleDto
                {
                    Title = "Global Warming Trends",
                    Author = "Prof. John Doe",
                    Year = 2022,
                    Publisher = "ScienceDirect",
                    Doi = "10.5678/exampledoi2",
                    Url = "https://example.com/article2"

                }
            };

            return Ok(results);
        }
    }
}
