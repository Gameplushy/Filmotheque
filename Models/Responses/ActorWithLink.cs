namespace Filmotheque.Models.Responses
{
    public class ActorWithLink
    {
        public required string Link { get; set; }
        public required Actor Actor { get; set; }
    }
}
