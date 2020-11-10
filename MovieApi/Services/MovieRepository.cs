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
        public async Task<string> UpdateMovie(Movie movie)
        {
            _ctx.Movies.Update(movie);
            await _ctx.SaveChangesAsync();
            return "Movie successfully updated";
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
                ReleaseDate = movie.ReleaseDate,
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
        public async Task<string> AddMovie(MovieDTO movie)
        {
            var film = await _ctx.Movies.FirstOrDefaultAsync(s => s.Name == movie.Name);
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
                    OwnerId = movie.OwnerId,
                    MovieGenres = movieGenres
                };
                await _ctx.Movies.AddAsync(newMovie);
                await _ctx.SaveChangesAsync();

                return newMovie.MovieId;

            }
        }

        // method to get movie by id
        public async Task<MoviesToReturn> GetMovieById(string Id)
        {
            // get specific movie from db
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == Id);
            if (movie == null)
            {
                return null;
            }
            // get genreIds from db
            var genreIds = await _ctx.MovieGenres.Where(e => e.MovieId == Id).ToListAsync();
            var genres = new List<string>();
            // add genres to genres list
            foreach (var id in genreIds)
            {
                var genre = await _ctx.Genres.FirstOrDefaultAsync(a => a.GenreId == id.GenreId);
                genres.Add(genre.Name);
            }
            var movieToReturn = new MoviesToReturn
            {
                Name = movie.Name,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = int.Parse(movie.Rating),
                TicketPrice = movie.TicketPrice,
                Country = movie.Country,
                PhotoUrl = movie.PhotoUrl,
                OwnerId = movie.OwnerId,
                Genres = genres
            };
            return movieToReturn;
        }

        //This method removes movies from the database
        public async Task<bool> RemoveMovie(string Id)
        {

            var film = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == Id);
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
