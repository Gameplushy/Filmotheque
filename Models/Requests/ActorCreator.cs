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
            if (FirstName == null || LastName == null || BirthDate == null) throw new Exception("You cannot fully convert this to an Actor yet, there are missing attributes.");
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
