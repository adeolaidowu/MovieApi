using MovieApi.Models;
using System.Collections.Generic;

namespace MovieApi.DTOs
{
    public class UpdateGenre
    {
        public string GenreId { get; set; }
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }

    }
}
