using MovieApi.DTOs;
using System.Collections.Generic;
using MovieApi.Models;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<string> AddMovie(MovieDTO movie);
        Task<bool> RemoveMovie(string Id);
        Task<string> UpdateMovie(UpdateMovieDto model, string Id);
        Task<IEnumerable<MoviesToReturn>> GetAllMovies(int pageNumber, int perPage);
        Task<MovieDTO> GetMovieById(string Id);
    }
}
