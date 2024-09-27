namespace EventsWebApplication.Data.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<Event> Events { get; set; }
        public List<UserEventTime> UserEvents { get; set; }
    }
}
