﻿using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using System.Collections.Generic;
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

        public async Task<List<Movie>> GetMovies()
        {
            return await _ctx.Movies.Include(x => x.MovieGenres).ToListAsync();
        }
    }
}
