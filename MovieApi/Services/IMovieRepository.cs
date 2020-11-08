﻿using MovieApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMovies();
    }
}
