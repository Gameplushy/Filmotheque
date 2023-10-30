namespace Filmotheque.Models.Responses
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ReleaseDate { get; set; }
        public required List<ActorWithLink> Actors { get; set; }
        public required List<DirectorWithLink> Directors { get; set; }
    }
}
