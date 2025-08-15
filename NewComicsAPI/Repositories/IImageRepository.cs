using NewComicsAPI.Models;

namespace NewComicsAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image img);
    }
}
