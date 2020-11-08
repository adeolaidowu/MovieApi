using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Services;
using System;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
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
        // This action is responsible for removing a movie from the database 
        [HttpDelete("RemoveMovie/{Id}")]
        public IActionResult RemoveMovie(string Id)

        {
            if (_movieRepository.RemoveMovie(Id))
            {
                return Ok("success");
            }
            return BadRequest();
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
                return BadRequest(e.Message);

            }
        }

    }
}
