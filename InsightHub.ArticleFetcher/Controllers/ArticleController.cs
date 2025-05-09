using InsightHub.ArticleFetcher.Models;
using InsightHub.ArticleFetcher.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace InsightHub.ArticleFetcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
      
        private readonly SemanticScholarService _semanticScholarService;
        
        public ArticleController(SemanticScholarService semanticScholarService)
        {
            
            
            _semanticScholarService = semanticScholarService;

        }




        [HttpGet("semantic")]
        public async Task<ActionResult<List<BasicArticleDto>>> SemanticSearch(
            [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var results = await _semanticScholarService.SearchAsync(query);

            if (!results.Any())
                return NotFound("No results from Semantic Scholar");

            return Ok(results);
        }
        
    }
}
