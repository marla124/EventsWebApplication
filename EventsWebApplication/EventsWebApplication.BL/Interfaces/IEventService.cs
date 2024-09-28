using EventsWebApplication.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.BL.Interfaces
{
    public interface IEventService : IService<EventDto>
    {
        Task DeleteById(Guid id, Guid userId, CancellationToken cancellationToken);
    }
}
