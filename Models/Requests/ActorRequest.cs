namespace Filmotheque.Models.Requests
{
    public class ActorRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public Actor ToActor()
        {
            return new Actor()
            {
                Id = 0,
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate
            };
        }
    }
}
