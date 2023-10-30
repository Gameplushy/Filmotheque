using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models.Requests
{
    public class ActorEditor
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
