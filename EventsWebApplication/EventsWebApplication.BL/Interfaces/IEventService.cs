using EventsWebApplication.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Migrations;

namespace EventsWebApplication.BL.Interfaces
{
    public interface IEventService : IService<EventDto>
    {
        Task AddParticipantToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task DeleteParticipantFromEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task DeleteById(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<List<UserDto>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken);
        Task<UserDto> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken);
        Task<UserDto> GetByName(string name, CancellationToken cancellationToken);

        Task<List<EventDto>?> GetEventsByCriteria(DateTime? date, string? address, string? categoryName,
            CancellationToken cancellationToken);
    }
}
