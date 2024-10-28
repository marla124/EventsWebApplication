﻿namespace EventsWebApplication.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public string Role { get; set; }
        public List<User>? Users { get; set; }
    }
}