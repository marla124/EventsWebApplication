﻿using System;

namespace EventsWebApplication.BL.Dto
{
    public class UpdateEventDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAndTime { get; set; }
        public Guid? CategoryId { get; set; }
        public int? MaxNumberOfPeople { get; set; }
        public string? Address { get; set; }
        public byte[]? Image { get; set; }
    }
}