using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticantController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAddParticipantToEventUseCase _addParticipantToEventUseCase;
        private readonly IDeleteParticipantFromEventUseCase _deleteParticipantFromEventUseCase;
        private readonly IGetEventParticipantByIdUseCase _getEventParticipantByIdUseCase;
        private readonly IGetEventParticipantsUseCase _getEventParticipantsUseCase;
        public PracticantController(IMapper mapper, IAddParticipantToEventUseCase addParticipantToEventUseCase,
            IDeleteParticipantFromEventUseCase deleteParticipantFromEventUseCase, 
            IGetEventParticipantByIdUseCase getEventParticipantByIdUseCase,
            IGetEventParticipantsUseCase getEventParticipantsUseCase)
        {
            _mapper = mapper;
            _addParticipantToEventUseCase = addParticipantToEventUseCase;
            _getEventParticipantByIdUseCase = getEventParticipantByIdUseCase;
            _deleteParticipantFromEventUseCase = deleteParticipantFromEventUseCase;
            _getEventParticipantsUseCase = getEventParticipantsUseCase;
        }

        [HttpPost("[action]/{eventId}")]
        [Authorize]
        public async Task<IActionResult> AddParticipantToEvent(Guid eventId, CancellationToken cancellationToken)
        {
                var userId = Guid.Parse(GetUserId());
                await _addParticipantToEventUseCase.Execute(userId, eventId, cancellationToken);
                return Ok();
        }

        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteParticipantFromEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(GetUserId());
            await _deleteParticipantFromEventUseCase.Execute(userId, eventId, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var practicants = await _getEventParticipantsUseCase.Execute(eventId, cancellationToken);
            return Ok(_mapper.Map<List<UserViewModel>>(practicants));
        }

        [HttpGet("[action]/{eventId, userId}")]
        public async Task<IActionResult> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var practicant = await _getEventParticipantByIdUseCase.Execute(eventId, userId, cancellationToken);
            return Ok(practicant);
        }
    }
}
