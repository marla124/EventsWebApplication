using EventsWebApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure
{
    public class EventWebApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserEventTime> UserEventsTime { get; set; }

        public EventWebApplicationDbContext(DbContextOptions<EventWebApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEventTime>()
                .HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<UserEventTime>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(ue => ue.UserId);

            modelBuilder.Entity<UserEventTime>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(ue => ue.EventId);
        }
    }
}
