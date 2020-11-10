using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Helper;
using MovieApi.Models;
using MovieApi.Services;
using System;
using System.Collections.Generic;
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
        private readonly UploadToCloudinary _UploadToCloudinary;

        public MovieController(ILogger<MovieController> logger, AppDbContext ctx, IMovieRepository movieRepository, IOptions<CloudinarySettings> cloudinary)
        {
            _logger = logger;
            _ctx = ctx;
            _movieRepository = movieRepository;
            _UploadToCloudinary = new UploadToCloudinary(cloudinary);
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
        public async Task<IActionResult> UpdateMovie([FromForm] UpdateMovieDto model, string id)
        {
            if (!ModelState.IsValid) return BadRequest();
            // instance of an imageUploadResult class
            ImageUploadResult uploadResult; /*= new ImageUploadResult();*/

            // find movie in database
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == id);
            var file = model.PhotoUrl;
            try
            {
             uploadResult = await _UploadToCloudinary.UploadPhoto(file);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            // confirm movie was found
            if (movie == null) return BadRequest("failed to update movie");

            movie.Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country;
            movie.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description;
            movie.Rating = !string.IsNullOrEmpty(model.Rating) ? model.Rating : movie.Rating;
            movie.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name;
            movie.TicketPrice = model.TicketPrice > 0 ? model.TicketPrice : movie.TicketPrice;
            movie.PhotoUrl = file.Length > 0 ? uploadResult.Url.ToString() : movie.PhotoUrl;
            movie.ReleaseDate = !string.IsNullOrEmpty(model.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate;

            try
            {

             var result = await _movieRepository.UpdateMovie(movie);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }

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
                    PhotoUrl = file.Length > 0 ? uploadResult.Url.ToString() : movie.PhotoUrl,
                    ReleaseDate = !string.IsNullOrEmpty(model.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate,
                    Genres = genres
                };
                return Ok(movieToReturn);
        }
            
    
        // This action is responsible for adding movies to the database
        [HttpPost("AddMovie")]

        public IActionResult AddMovie([FromForm] MovieDTO movie)
        {
            if (ModelState.IsValid)
            {
                Task<string> response = null;
                try
                {
                    response = _movieRepository.AddMovie(movie);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest("Failed to add movie");
                }

                if (response != null)
                {
                    return Ok(response);
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
