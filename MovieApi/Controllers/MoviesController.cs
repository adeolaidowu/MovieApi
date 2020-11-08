using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MoviesController> _logger;


        public MoviesController(IMovieRepository movieRepository, ILogger<MoviesController> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }
        [HttpGet("{pageNumber?}")]
        public async Task<IActionResult> GetAllMovies(int perPage = 6, int pageNumber = 1)
        {
            try
            {
                if (pageNumber <= 0) return BadRequest();

                var movies = await _movieRepository.GetMovies(pageNumber, perPage);
                var count = movies.Count();

                return Ok(new { perPage, pageNumber, count, movies });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return StatusCode(500, new { Error = e.Message });
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMovie(string Id)
        {
            var movie = await _movieRepository.GetMovieById(Id);

            return Ok(new { Movie = movie });
        }

    }
}
