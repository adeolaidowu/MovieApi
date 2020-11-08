using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _ctx;

        public GenreRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Genre>> FetchGenres()
        {
            return await _ctx.Genres.ToListAsync();
        }
    }
}
