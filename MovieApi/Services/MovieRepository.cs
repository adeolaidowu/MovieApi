using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _ctx;

        public MovieRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<string> UpdateMovie(string Id, UpdateMovieDto model)
        {
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == Id);

            if (movie == null) return "failed to update movie";


            movie.Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country;
            movie.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description;
            movie.Rating = model.Rating > 0 ? model.Rating : movie.Rating;
            movie.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name;
            movie.TicketPrice = model.TicketPrice > 0 ? model.TicketPrice : movie.TicketPrice;
            movie.PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? model.PhotoUrl : movie.PhotoUrl;
            movie.ReleaseDate = !model.ReleaseDate.Equals(movie.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate;

            _ctx.Movies.Update(movie);
            await _ctx.SaveChangesAsync();

            return "Update done successfully";
        }
        //This method is what removes a movie from the database.
        public string AddMovie(MovieDTO movie)
        {
            var film = _ctx.Movies.FirstOrDefault(s => s.Name == movie.Name);
            if (film != null)
            {
                return null;
            }
            else
            {
                List<MovieGenre> movieGenres = new List<MovieGenre>();
                foreach (var genre in movie.Genres)
                {
                    movieGenres.Add(new MovieGenre { GenreId = genre });
                }

                var newMovie = new Movie
                {// The movieDTo is used to populate the database
                    MovieId = Guid.NewGuid().ToString(),// The guid method is used to create Id for the MovieId
                    Name = movie.Name,
                    Description = movie.Description,
                    ReleaseDate = DateTime.Parse(movie.ReleaseDate),
                    Rating = int.Parse(movie.Rating),
                    TicketPrice = movie.TicketPrice,
                    Country = movie.Country,
                    PhotoUrl = movie.PhotoUrl,
                    MovieGenres = movieGenres
                };
                _ctx.Movies.Add(newMovie);
                _ctx.SaveChanges();

                return newMovie.MovieId;

            }
        }
        //This method removes movies from the database
        public bool RemoveMovie(string Id)
        {

            var film = _ctx.Movies.FirstOrDefault(x => x.MovieId == Id);
            if (film == null)
            {
                return false;
            }
            else
            {
                _ctx.Movies.Remove(film);
                _ctx.SaveChanges();
                return true;
            }
        }
    }
}
