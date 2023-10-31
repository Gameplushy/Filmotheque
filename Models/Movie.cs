using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ReleaseDate { get; set; }
        public required List<Actor> Actors { get; set; }
        public required List<Director> Directors { get; set; }
    }
}