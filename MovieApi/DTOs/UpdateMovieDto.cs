using Microsoft.AspNetCore.Http;

namespace MovieApi.DTOs
{
    public class UpdateMovieDto
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Rating { get; set; }
        public double TicketPrice { get; set; }
        public string Country { get; set; }
        public IFormFile PhotoUrl { get; set; }
    }
}
