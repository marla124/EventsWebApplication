namespace EventsWebApplication.Data.Entities
{
    public class UserEventTime
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime RegistrationDate { get; set; }
    }

}
