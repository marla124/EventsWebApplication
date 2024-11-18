using EventsWebApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task AddParticipantToEvent(UserEventTime userEventTime, CancellationToken cancellationToken);
        Task<int> GetEventParticipantsCount(Guid eventId, CancellationToken cancellationToken);
        void DeleteParticipantFromEvent(UserEventTime userEventTime);
        Task<List<User>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken);
    }
}
