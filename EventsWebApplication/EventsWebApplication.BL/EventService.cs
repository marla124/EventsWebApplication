using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.BL
{
    public class EventService : Service<EventDto, Event>, IEventService
    {
        public EventService(IRepository<Event> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
