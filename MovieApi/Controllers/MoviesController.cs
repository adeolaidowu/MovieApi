using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.DTOs;
using MovieApi.Services;
using System;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MoviesController> _logger;


        public MoviesController(IMovieRepository movieRepository, IGenreRepository genreRepository, ILogger<MoviesController> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieDto model, string ID)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {

                var result = await _movieRepository.UpdateMovie(ID, model);


                return Ok(new { result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Error = e.Message });

            }
        }


    }
}
