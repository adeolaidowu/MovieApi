using MovieApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IGenreRepository
    {
        Task<List<Genre>> FetchGenres();
    }
}