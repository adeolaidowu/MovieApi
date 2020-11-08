using Microsoft.AspNetCore.Mvc;
using MovieApi.Services;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        public MovieController(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var services = new MovieService(_movieRepository, _genreRepository);
            var result = await services.FetchAllMovies();
            return Ok(new { result });
        }
    }
}
