namespace Filmotheque.Models.Responses
{
    public class DirectorWithLink
    {
        public required string Link { get; set; }
        public required Director Director { get; set; }
    }
}
