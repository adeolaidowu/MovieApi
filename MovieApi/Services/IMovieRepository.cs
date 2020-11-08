﻿using MovieApi.DTOs;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IMovieRepository
    {
        public string AddMovie(MovieDTO movie);
        public bool RemoveMovie(string Id);
        Task<string> UpdateMovie(string Id, UpdateMovieDto model);
    }
}