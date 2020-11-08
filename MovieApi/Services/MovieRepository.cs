using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IGenreRepository _genreRepository;

        public MovieRepository(AppDbContext ctx, IGenreRepository genreRepository)
        {
            _ctx = ctx;
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<MoviesToReturn>> GetMovies(int pageNumber, int perPage)
        {
            var movieIdAndGenres = new Dictionary<string, List<string>>();

            var moviesCollection = await _ctx.Movies.Include(x => x.MovieGenres).ToListAsync();
            var genresCollection = await _genreRepository.FetchGenres();

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
                    ;
                }

            }

            var result = moviesCollection.Select(movie => new MoviesToReturn
            {
                MovieId = movie.MovieId,
                Genres = movieIdAndGenres[movie.MovieId],
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Description = movie.Description,
                PhotoUrl = movie.PhotoUrl,
                Country = movie.Country,
                Name = movie.Name,
                TicketPrice = movie.TicketPrice

            });
            return result.Skip((pageNumber - 1) * perPage).Take(perPage);
        }

        public async Task<MoviesToReturn> GetMovieById(string id)
        {


            var movieData = await _ctx.Movies.Include(x => x.MovieGenres).FirstOrDefaultAsync(x => x.MovieId == id); ;
            var movieAndGenre = movieData.MovieGenres;
            var movieIdAndGenres = new Dictionary<string, List<string>>();


            var genresCollection = await _genreRepository.FetchGenres();




            var joinMovieAndGenre = movieAndGenre.Join(genresCollection,
                x => x.GenreId,
                y => y.GenreId,
                ((movie, genre) => new { MovieID = movie.MovieId, Genre = genre.Name }));


            foreach (var movie in joinMovieAndGenre)
            {
                if (!movieIdAndGenres.ContainsKey(movie.MovieID))
                {
                    movieIdAndGenres[movie.MovieID] = new List<string> { movie.Genre };
                }
                else
                {
                    movieIdAndGenres[movie.MovieID].Add(movie.Genre);

                }

            }

            var result = new MoviesToReturn
            {
                MovieId = movieData.MovieId,
                Genres = movieIdAndGenres[movieData.MovieId],
                ReleaseDate = movieData.ReleaseDate,
                Rating = movieData.Rating,
                Description = movieData.Description,
                PhotoUrl = movieData.PhotoUrl,
                Country = movieData.Country,
                Name = movieData.Name,
                TicketPrice = movieData.TicketPrice

            };

            return result;

        }




    }
}
