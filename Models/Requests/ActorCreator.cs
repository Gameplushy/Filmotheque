using System.ComponentModel.DataAnnotations;

namespace Filmotheque.Models.Requests
{
    public class ActorCreator
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required DateTime BirthDate { get; set; }

        public Actor ToActor()
        {
            return new Actor()
            {
                Id = 0,
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = (DateTime)BirthDate
            };
        }
    }
}
