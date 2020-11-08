using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.DTOs
{
    public class MovieDTO
    {        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
        [Required]
        [Range(1, 5)]
        public string Rating { get; set; }
        [Required]
        public List<string> Genres { get; set; }
        [Required]
        public double TicketPrice { get; set; }
        [Required]
        public string Country { get; set; }

        public string PhotoUrl { get; set; }

        public MovieDTO()
        {
            Genres = new List<string>();
            
        }
    }
}
