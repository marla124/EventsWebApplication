using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.BL
{
    public class EventService : Service<EventDto, Event>, IEventService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IMapper mapper, IUserService userService,
            IUnitOfWork unitOfWork) : base(unitOfWork.EventRepository, mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            await _unitOfWork.EventRepository.DeleteById(id, cancellationToken);
            await _unitOfWork.EventRepository.Commit(cancellationToken);
        }

        public async Task<UpdateEventDto> Update(UpdateEventDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var role = await _userService.GetUserRole(userId, cancellationToken);
            var existingEvent = await _unitOfWork.EventRepository
                .GetAsQueryable()
                .FirstOrDefaultAsync(e => e.Id == dto.Id, cancellationToken);

            if (existingEvent == null)
            {
                throw new KeyNotFoundException();
            }

            existingEvent.Name = dto.Name ?? existingEvent.Name;
            existingEvent.Description = dto.Description ?? existingEvent.Description;
            existingEvent.DateAndTime = dto.DateAndTime ?? existingEvent.DateAndTime;
            existingEvent.MaxNumberOfPeople = dto.MaxNumberOfPeople ?? existingEvent.MaxNumberOfPeople;
            existingEvent.Address = dto.Address ?? existingEvent.Address;
            existingEvent.CategoryId = dto.CategoryId ?? existingEvent.CategoryId;
            existingEvent.Image = dto.Image ?? existingEvent.Image;
            existingEvent.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _unitOfWork.EventRepository.Update(existingEvent, cancellationToken);
                await _unitOfWork.EventRepository.Commit(cancellationToken);
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }


        public override async Task<EventDto> Create(EventDto dto, CancellationToken cancellationToken)
        {
            var eventInfo = new Event()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                DateAndTime = dto.DateAndTime,
                CategoryId = dto.CategoryId,
                UserCreatorId = dto.UserCreatorId,
                MaxNumberOfPeople = dto.MaxNumberOfPeople,
                Address = dto.Address,
                Image = dto.Image,
                CreatedAt = DateTime.UtcNow,
            };
            var createdEntity = await _unitOfWork.EventRepository.CreateOne(eventInfo, cancellationToken);
            await _unitOfWork.EventRepository.Commit(cancellationToken);
            return _mapper.Map<EventDto>(createdEntity);
        }

        public async Task AddParticipantToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            await _unitOfWork.EventRepository.AddParticipantToEvent(userId, eventId, cancellationToken);
        }

        public async Task DeleteParticipantFromEvent(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            await _unitOfWork.EventRepository.DeleteParticipantFromEvent(userId, eventId, cancellationToken);
        }

        public async Task<List<UserDto>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.EventRepository.GetEventParticipants(eventId, cancellationToken);
            return _mapper.Map<List<UserDto>?>(users);
        }

        public async Task<UserDto> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.EventRepository.GetEventParticipantById(eventId, userId, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<EventDto> GetByName(string name, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.EventRepository.GetByName(name, cancellationToken);
            return _mapper.Map<EventDto>(user);
        }

        public async Task<List<EventDto>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByCriteria(date, address, categoryId, cancellationToken);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetUsersEvents(userId, cancellationToken);
            return _mapper.Map<List<EventDto>?>(events);
        }

        public async Task UploadImage(Guid eventId, Guid userId, IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException();
            }

            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var eventToUpdate = _mapper.Map<UpdateEventDto>(await GetById(eventId, cancellationToken));
            if (eventToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            eventToUpdate.Image = imageData;
            await Update(eventToUpdate, userId, cancellationToken);
        }
    }
}
