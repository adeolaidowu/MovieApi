using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly AppDbContext _ctx;
        private readonly IMovieRepository _movieRepository;

        public MovieController(ILogger<MovieController> logger, AppDbContext ctx, IMovieRepository movieRepository)
        {
            _logger = logger;
            _ctx = ctx;
            _movieRepository = movieRepository;
        }
        [AllowAnonymous]
        [HttpGet("getmovies/{pageNumber?}")]
        public async Task<IActionResult> GetAllMovies(int perPage = 6, int pageNumber = 1)
        {
            try
            {
                if (pageNumber <= 0) return BadRequest();
                var movies = await _movieRepository.GetAllMovies(pageNumber, perPage);
                var count = movies.Count();

                return Ok(new { perPage, pageNumber, count, movies });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        // This action updates a movie in the database

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieDto model, string id)

        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var result = await _movieRepository.UpdateMovie(model, id);
                return Ok(new { result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        // This action is responsible for adding movies to the database
        [HttpPost("AddMovie")]

        public IActionResult AddMovie([FromBody] MovieDTO movie)
        {
            if (ModelState.IsValid)
            {
                var response = _movieRepository.AddMovie(movie);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("error adding movie");
                }

            }
            return BadRequest("Incomplete data");
        }
        // This action is responsible for fetching a movie to the database
        [HttpGet("GetMovie/{id}")]

        public IActionResult GetMovieById(string Id)
        {
            var response = _movieRepository.GetMovieById(Id);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("error fetching specified movie");
            }
        }
        // This action is responsible for removing a movie from the database 
        [HttpDelete("RemoveMovie/{Id}")]
        public async Task<IActionResult> RemoveMovie(string Id)

        {
            if (await _movieRepository.RemoveMovie(Id))
            {
                return Ok("success");
            }
            return BadRequest();
        }
    }
}
