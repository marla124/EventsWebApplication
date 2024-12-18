﻿namespace EventsWebApplication.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<Event>? Events { get; set; }
    }
}
