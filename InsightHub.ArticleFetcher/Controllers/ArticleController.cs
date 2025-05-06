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
        private readonly UnpaywallService _unpaywallService;

        public ArticleController(UnpaywallService unpaywallService)
        {
            _unpaywallService = unpaywallService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<ArticleDto>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required.");

            var result = await _unpaywallService.GetRawArticlesAsync(query);

            if (result == null)
                return NotFound("No article found or failed to fetch.");

            return Ok(result);
        }

        [HttpGet("{doi}")]
        public async Task<ActionResult<DetailedArticleDto>> GetByDoi(string doi)
        {
      

            if (string.IsNullOrWhiteSpace(doi))
                return BadRequest("DOI  is required");

            var decodeDoi = HttpUtility.UrlDecode(doi);

            var result = await _unpaywallService.GetArticleByDoiAsync(decodeDoi);

            if (result == null)
                return NotFound($"No article found for DOI: {doi}");

            return Ok(result);
        }
    }
}
