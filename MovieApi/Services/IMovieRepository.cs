using MovieApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MoviesToReturn>> GetMovies(int pageNumber, int perPage);
        Task<MoviesToReturn> GetMovieById(string id);
    }
}