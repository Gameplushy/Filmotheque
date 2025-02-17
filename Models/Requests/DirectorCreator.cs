using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models.Requests
{
    public class DirectorCreator
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required DateTime BirthDate { get; set; }

        public Director ToDirector()
        {
            return new Director()
            {
                Id = 0,
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate
            };
        }
    }
}
