using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApi.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _ctx;

        public MovieRepository(AppDbContext ctx)
        {
            _ctx = ctx;
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
                    ReleaseDate = movie.ReleaseDate,
                    Rating = movie.Rating,
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
    
        // method to get movie by id
        public Movie GetMovieById(string Id)
        {
            var movie = _ctx.Movies.FirstOrDefault(x => x.MovieId == Id);
            if (movie == null)
            {
                return null;
            }
            return movie;
        }

        // method to get all movies
        public List<Movie> GetAllMovies()
        {
            var allmovies = _ctx.Movies.ToList();
            if (allmovies.Count == 0)
            {
                return null;
            }
            return allmovies;
        }
    }
}
