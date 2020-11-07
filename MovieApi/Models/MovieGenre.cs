using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Models
{
    public class MovieGenre
    {
        //public int Id { get; set; }
        public string MovieId { get; set; }
        public Movie Movies { get; set; }

        public string GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
