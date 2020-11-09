using System;
using System.Collections.Generic;

namespace MovieApi.DTOs
{
    public class MoviesToReturn
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public int Rating { get; set; }
        public double TicketPrice { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }

        public List<string> Genres { get; set; }
    }
}
