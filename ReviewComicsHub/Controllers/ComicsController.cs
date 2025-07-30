using ComicsAPI.DTOs;
using ComicsAPI.Mapper;
using ComicsAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComicsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicsController : ControllerBase
    {
        private readonly IComicsRepository _comicsRepo;

        public ComicsController(IComicsRepository comicsRepo)
        {
            _comicsRepo = comicsRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComics()
        {
            var ComicsDomain = await _comicsRepo.GetAllComicsAsync();

            var ComicsDto = ComicsDomain.Select(comic => ComicMapper.MapToDto(comic)).ToList();

            return Ok(ComicsDto);
        }

        [HttpGet("{comicId}")]
        public async Task<IActionResult> GetComicIssues(int comicId)
        {
            try
            {
                var comicWithIssues = await _comicsRepo.GetComicIssuesAsync(comicId);
                if (comicWithIssues == null)
                {
                    return NotFound($"Comic with ID {comicId} not found.");
                }
                return Ok(comicWithIssues);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{comicId}")]
        public async Task<IActionResult> ChangeComicTitle([FromRoute] int comicId, [FromBody] string newName)
        {
            var updatedComicDomain = await _comicsRepo.UpdateComicTitleAsync(comicId, newName);
            if (updatedComicDomain == null)
            {
                return NotFound($"Comic with ID {comicId} not found.");
            }
            var updatedComicDto = ComicMapper.MapToDto(updatedComicDomain);
            return Ok(updatedComicDto);
        }

 }
}
