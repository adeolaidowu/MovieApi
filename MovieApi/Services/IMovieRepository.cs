using MovieApi.DTOs;
using System.Collections.Generic;
using MovieApi.Models;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        public string AddMovie(MovieDTO movie);
        public bool RemoveMovie(string Id);
        Task<string> UpdateMovie(UpdateMovieDto model, string Id);
        Task<IEnumerable<MoviesToReturn>> GetAllMovies(int pageNumber, int perPage);
        public MovieDTO GetMovieById(string Id);
    }
}
