using EventsWebApplication.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.BL.Interfaces
{
    public interface IEventService : IService<EventDto>
    {
        Task AddParticipantToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task DeleteParticipantFromEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task DeleteById(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<List<UserDto>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken);
        Task<UserDto> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken);
        Task<EventDto> GetByName(string name, CancellationToken cancellationToken);

        Task<List<EventDto>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken);

        Task<List<EventDto>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken);
        Task<UpdateEventDto> Update(UpdateEventDto dto, Guid userId, CancellationToken cancellationToken);
        Task UploadImage(Guid eventId, Guid userId, IFormFile file, CancellationToken cancellationToken);
    }
}
