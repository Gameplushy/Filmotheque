namespace Filmotheque.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ReleaseDate { get; set; }
        public required List<Person> Actors { get; set; }
        public required List<Person> Directors { get; set; }
    }
}