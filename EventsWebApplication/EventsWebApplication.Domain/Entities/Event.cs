namespace EventsWebApplication.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAndTime { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public Guid UserCreatorId { get; set; }
        public string? Address { get; set; }
        public byte[]? Image { get; set; }
        public List<UserEventTime>? UserEvents { get; set; }
    }
}
