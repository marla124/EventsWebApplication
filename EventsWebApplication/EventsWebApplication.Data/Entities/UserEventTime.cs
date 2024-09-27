namespace EventsWebApplication.Data.Entities
{
    public class UserEventTime
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public DateTime RegistrationDate { get; set; }
    }

}
