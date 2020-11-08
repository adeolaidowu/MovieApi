using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
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


        public async Task<string> UpdateMovie(string id, UpdateMovieDto model)
        {
            var movie = await _ctx.Movies.FirstOrDefaultAsync(x => x.MovieId == id);

            if (movie == null) return "movie not found";


            movie.Country = !string.IsNullOrEmpty(model.Country) ? model.Country : movie.Country;
            movie.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : movie.Description;
            movie.Rating = model.Rating > movie.Rating ? model.Rating : movie.Rating;
            movie.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : movie.Name;
            movie.TicketPrice = model.TicketPrice > movie.TicketPrice ? model.TicketPrice : movie.TicketPrice;
            movie.PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? model.PhotoUrl : movie.PhotoUrl;
            movie.ReleaseDate = !model.ReleaseDate.Equals(movie.ReleaseDate) ? model.ReleaseDate : movie.ReleaseDate;



            return "Update done successfully";
        }




    }
}
