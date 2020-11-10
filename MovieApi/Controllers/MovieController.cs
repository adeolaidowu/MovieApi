using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            var userId = VerifyToken(HttpContext);


            if (!ModelState.IsValid) return BadRequest();
            try
            {
                // find movie in database
                var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == id);

                // confirm movie was found
                if (movie == null) return BadRequest("failed to update movie");

                var isVerified = MatchUserIdOwnerId(userId, movie.OwnerId);

                if (!isVerified)
                {
                    return BadRequest("You are not authorized to update this movie");
                }


                movie.Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country;
                movie.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description;
                movie.Rating = !string.IsNullOrEmpty(model.Rating) ? model.Rating : movie.Rating;
                movie.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name;
                movie.TicketPrice = model.TicketPrice > 0 ? model.TicketPrice : movie.TicketPrice;
                movie.PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? model.PhotoUrl : movie.PhotoUrl;
                movie.ReleaseDate = !string.IsNullOrEmpty(model.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate;

                var result = await _movieRepository.UpdateMovie(movie);

                // get genreIds from db
                var genreIds = await _ctx.MovieGenres.Where(e => e.MovieId == id).ToListAsync();
                var genres = new List<string>();
                // add genres to genres list
                foreach (var Id in genreIds)
                {
                    var genre = await _ctx.Genres.FirstOrDefaultAsync(a => a.GenreId == Id.GenreId);
                    genres.Add(genre.Name);
                }


                //create movie to return as payload
                var movieToReturn = new MoviesToReturn
                {
                    MovieId = movie.MovieId,
                    Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country,
                    Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description,
                    Rating = !string.IsNullOrEmpty(model.Rating) ? Int32.Parse(model.Rating) : Int32.Parse(movie.Rating),
                    Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name,
                    TicketPrice = model.TicketPrice > 0 ? model.TicketPrice : movie.TicketPrice,
                    PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? model.PhotoUrl : movie.PhotoUrl,
                    ReleaseDate = !string.IsNullOrEmpty(model.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate,
                    Genres = genres
                };
                return Ok(movieToReturn);
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
                var userId = VerifyToken(HttpContext);

                var isVerified = MatchUserIdOwnerId(userId, movie.OwnerId);
                if (isVerified)
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
                return BadRequest("Cannot add movie");
               

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
            var userId = VerifyToken(HttpContext);

            var movie = await _movieRepository.GetMovieById(Id);



            var isVerified = MatchUserIdOwnerId(userId, movie.OwnerId);

            if (!isVerified) return BadRequest("You are not authorized to delete this movie");

            if (await _movieRepository.RemoveMovie(Id))
            {
                return Ok("success");
            }
            return BadRequest();
        }

        private string VerifyToken(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();

            var claims = handler.ReadJwtToken(token).Claims.Take(1);
            string userId = string.Empty;
            foreach (var claim in claims)
            {
                userId += claim.Value;
            }


            return userId;
        }

        private static bool MatchUserIdOwnerId(string userId, string ownerId)
        {
            if (userId == ownerId)
            {
                return true;

            }

            return false;

        }
    }
}
