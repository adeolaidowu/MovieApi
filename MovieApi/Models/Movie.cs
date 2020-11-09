using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Movie
    {
        [Key]
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Rating { get; set; }
        public double TicketPrice { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public string OwnerId { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
