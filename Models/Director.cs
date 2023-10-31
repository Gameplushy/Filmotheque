using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models
{
    public class Director
    {
        [Key]
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
