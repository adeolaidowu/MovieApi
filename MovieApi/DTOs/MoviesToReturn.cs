using MovieApi.Models;
using System;

namespace MovieApi.DTOs
{
    public class MoviesToReturn
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Rating { get; set; }
        public double TicketPrice { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }

        public Genre Genres { get; set; }
    }
}
