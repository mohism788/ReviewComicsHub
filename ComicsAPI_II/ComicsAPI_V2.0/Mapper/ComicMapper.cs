using ComicsAPI_V2.DTOs;
using ComicsAPI_V2.Models;

namespace ComicsAPI_V2.Mapper
{
    public class ComicMapper
    {
        //make mapping from Comics to ComicDto
        public static ComicDto MapToDto(Comics comic)
        {
            if (comic == null)
            {
                return null;
            }
            return new ComicDto
            {
                ComicId = comic.ComicId,
                Title = comic.Title,
                Description = comic.Description,
                CoverImageUrl = comic.CoverImageUrl
            };
        }

        //Map comic with Comic with Issues Dto
        public static ComicWithIssuesDto MapToComicWithIssuesDto(Comics comic, IEnumerable<IssueDto> issues)
        {
            if (comic == null || issues == null)
            {
                return null;
            }
            return new ComicWithIssuesDto
            {
                ComicId = comic.ComicId,
                Title = comic.Title,
                Description = comic.Description,
                Issues = issues
            };
        }

        //MapFromCreateDtoToDomain
        public static Comics MapFromCreateDtoToDomain(CreateNewComicDto createNewComicDto)
        {
            if (createNewComicDto == null)
            {
                return null;
            }
            return new Comics
            {
                Title = createNewComicDto.Title,
                Description = createNewComicDto.Description,
                CoverImageUrl = createNewComicDto.CoverImageUrl
            };
        }
    }
}
