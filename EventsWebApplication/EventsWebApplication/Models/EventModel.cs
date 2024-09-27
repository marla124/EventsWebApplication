namespace EventsWebApplication.Models
{
    public class CreateEventModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAndTime { get; set; }
        public string CategoryName { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public string? Address { get; set; }
        public byte[]? Image { get; set; }
    }
}
