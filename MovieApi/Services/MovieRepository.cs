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
        public async Task<string> UpdateMovie(UpdateMovieDto model, string Id)
        {
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == Id);

            if (movie == null) return "failed to update movie";


            movie.Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country;
            movie.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description;
            movie.Rating = model.Rating > 0 ? model.Rating.ToString() : movie.Rating;
            movie.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name;
            movie.TicketPrice = model.TicketPrice > 0 ? model.TicketPrice : movie.TicketPrice;
            movie.PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? model.PhotoUrl : movie.PhotoUrl;
            movie.ReleaseDate = !model.ReleaseDate.Equals(movie.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate;

            var updateMovieGenre = await _ctx.MovieGenres.FirstOrDefaultAsync(x => x.MovieId == movie.MovieId);
            updateMovieGenre.GenreId = !string.IsNullOrEmpty(model.GenreId) ? model.GenreId : updateMovieGenre.GenreId;

            _ctx.Movies.Update(movie);
            await _ctx.SaveChangesAsync();

            _ctx.MovieGenres.Update(updateMovieGenre);
            await _ctx.SaveChangesAsync();


            return "Update done successfully";
        }

        // Returns a list of movies 6 per page
        public async Task<IEnumerable<MoviesToReturn>> GetAllMovies(int pageNumber, int perPage)
        {
            var movieIdAndGenres = new Dictionary<string, List<string>>();
            var moviesCollection = await _ctx.Movies.Include(x => x.MovieGenres).ToListAsync();
            var genresCollection = await _ctx.Genres.ToListAsync();
            var moviesIdAndGenresId = moviesCollection.SelectMany(x => x.MovieGenres);

            var movieAndGenre = moviesIdAndGenresId.Join(genresCollection,
                m => m.GenreId,
                g => g.GenreId,
                ((movieGenre, genre) => new
                {
                    MovieId = movieGenre.MovieId,
                    Genre = genre.Name

                }));

            foreach (var movie in movieAndGenre)
            {
                if (!movieIdAndGenres.ContainsKey(movie.MovieId))
                {
                    movieIdAndGenres[movie.MovieId] = new List<string> { movie.Genre };
                }
                else
                {
                    movieIdAndGenres[movie.MovieId].Add(movie.Genre);
                }
            }

            var result = moviesCollection.Select(movie => new MoviesToReturn
            {
                MovieId = movie.MovieId,
                Genres = movieIdAndGenres[movie.MovieId],
                ReleaseDate = DateTime.Parse(movie.ReleaseDate),
                Rating = int.Parse(movie.Rating),
                Description = movie.Description,
                PhotoUrl = movie.PhotoUrl,
                Country = movie.Country,
                Name = movie.Name,
                TicketPrice = movie.TicketPrice

            });
            return result.Skip((pageNumber - 1) * perPage).Take(perPage);
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
    }
}
