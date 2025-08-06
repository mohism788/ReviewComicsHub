using ComicsAPI.DTOs;
using ComicsAPI.Mapper;
using ComicsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetAllComics()
        {
            var ComicsDomain = await _comicsRepo.GetAllComicsAsync();

            var ComicsDto = ComicsDomain.Select(comic => ComicMapper.MapToDto(comic)).ToList();

            return Ok(ComicsDto);
        }

        [HttpGet("{comicId}")]
        [Authorize]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Comic not found");
            }
        }

        [HttpPut("{comicId}")]
        [Authorize(Roles = "Moderator")]
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

        [HttpDelete]
        [Route("{comidId}")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteComicById([FromRoute] int comidId)
        {
            try
            {
                var deleted = await _comicsRepo.DeleteComicByIdAsync(comidId);
                if (!deleted)
                {
                    return NotFound($"Comic with ID {comidId} not found.");
                }
                return Ok($"Comic with ID {comidId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> CreateComic([FromBody] CreateNewComicDto createNewComicDto)
        {
            if (createNewComicDto == null)
            {
                return BadRequest("Comic data is null.");
            }
            //map
            var createNewComicDomain = ComicMapper.MapFromCreateDtoToDomain(createNewComicDto);
            var createdComic = await _comicsRepo.CreateComicAsync(createNewComicDomain);
            if (createdComic == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating comic.");
            }
            var createdComicDto = ComicMapper.MapToDto(createdComic);
            return CreatedAtAction(nameof(GetAllComics), new { comicId = createdComic.ComicId }, createdComicDto);

        }

    }
}
