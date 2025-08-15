using ComicsAPI.Models;

namespace ComicsAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image img);
    }
}
