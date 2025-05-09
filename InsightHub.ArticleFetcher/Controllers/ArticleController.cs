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
        private readonly LocalArticleIndex _localIndex;
        private readonly CrossRefService _crossRefService;

        public ArticleController(UnpaywallService unpaywallService, LocalArticleIndex localIndex, CrossRefService crossRefService)
        {
            _unpaywallService = unpaywallService;
            _localIndex = localIndex;
            _crossRefService = crossRefService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ArticleDto>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var dois = _localIndex.SearchDois(query);

            if (!dois.Any())
                return NotFound("No articles matched yours earch");

            var results = new List<ArticleDto>();

            foreach (var doi in dois)
            {
                var detailed = await _unpaywallService.GetArticleByDoiAsync(doi);

                if (detailed != null)
                {
                    var dto = new ArticleDto
                    {
                        Title = detailed.Title,
                        Authors = detailed.Authors,
                        Year = detailed.Year,
                        Publisher = detailed.Publisher,
                        Doi = detailed.Doi,
                        LandingPageUrl = detailed.LandingPageUrl
                    };

                    results.Add(dto);
                }
            }

            return Ok(results);
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

        [HttpGet("crossref")]
        public async Task<ActionResult<List<CrossRefDto>>> CrossRefSearch([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var results = await _crossRefService.SearchDoisAsync(query);

            if (!results.Any())
                return NotFound("No results from CrossRef.");

            return Ok(results);
        }

        [HttpGet("crossref/full")]
        public async Task<ActionResult<List<CrossRefDto>>> CrossRefFullSearch(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var results = await _crossRefService.SearchAndEnrichWithUnpaywallAsync(query, page, pageSize);

            if (!results.Any())
                return NotFound("No results from CrossRef.");

            return Ok(results);

               
        }
    }
}
