using MovieApi.DTOs;
using MovieApi.Models;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        public string AddMovie(MovieDTO movie);
        public bool RemoveMovie(string Id);
        Task<string> UpdateMovie(UpdateMovieDto model, string Id);

        public MovieDTO GetMovieById(string Id);
    }
}
