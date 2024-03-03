
using Microsoft.AspNetCore.Mvc;
using SantanderTestWebAPI.Interfaces;
using SantanderTestWebAPI.Models;

namespace SantanderTestWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly IHackerNewsFetcher _hackerNewsService;

        public BestStoriesController(IHackerNewsFetcher hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet("{count}")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<BestStory>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(int count)
        {
            try
            {
                if (count <= 0)
                {
                    return BadRequest("Count must be greater than zero.");
                }
                var bestStories = await _hackerNewsService.GetBestStoriesAsync(count);
                if (bestStories == null || !bestStories.Any())
                {
                    return NotFound("No best stories found.");
                }

                return Ok(bestStories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong while fetching stories: {ex.Message}");
            }
        }
    }
}