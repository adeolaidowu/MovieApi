using MovieApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        public string AddMovie(MovieDTO movie);
        public bool RemoveMovie(string Id);
    }
}
