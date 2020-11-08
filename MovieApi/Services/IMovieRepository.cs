using MovieApi.DTOs;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<string> UpdateMovie(string id, UpdateMovieDto model);

    }
}