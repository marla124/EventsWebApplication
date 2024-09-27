namespace EventsWebApplication.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiringAt { get; set; }
        public string AssociateDeviceName { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
