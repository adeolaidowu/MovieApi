using MovieApi.DTOs;
using MovieApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<MoviesToReturn> AddMovie(MovieDTO movie);
        Task<bool> RemoveMovie(string Id);
        Task<string> UpdateMovie(Movie movie);
        Task<IEnumerable<MoviesToReturn>> GetAllMovies(int pageNumber, int perPage);
        Task<MoviesToReturn> GetMovieById(string Id);
    }
}
