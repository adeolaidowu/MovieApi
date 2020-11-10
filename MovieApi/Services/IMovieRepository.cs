﻿using MovieApi.DTOs;
using System.Collections.Generic;
using MovieApi.Models;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<string> AddMovie(MovieDTO movie);
        Task<bool> RemoveMovie(string Id);
        Task<string> UpdateMovie(Movie movie);
        Task<IEnumerable<MoviesToReturn>> GetAllMovies(int pageNumber, int perPage);
        Task<MoviesToReturn> GetMovieById(string Id);
    }
}
