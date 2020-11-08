using Microsoft.EntityFrameworkCore.Internal;
using MovieApi.DTOs;
using MovieApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }
        public async Task<List<Movie>> FetchAllMovies()
        {
            var movies = await _movieRepository.GetMovies();
            var genres = await _genreRepository.FetchGenres();

            var movieGenres = movies.SelectMany(x => x.MovieGenres);


            var result = movieGenres.Join(genres,
                m => m.GenreId,
                g => g.GenreId,
                ((mg, genre) => new MoviesToReturn
                {
                    MovieId = mg.MovieId,
                    MovieGenres =
                }));



        }
    }
}
