using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories;
using EventsWebApplication.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.BL
{
    public class EventService : Service<EventDto, Event>, IEventService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IEventRepository repository, IMapper mapper, IUserService userService,
            IUnitOfWork unitOfWork) : base(repository, mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteById(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var eventInfo = await GetById(id, cancellationToken);
            var role = await _userService.GetUserRole(userId, cancellationToken);
            if (eventInfo.UserCreatorId == userId || role.Role == "Admin")
            {
                await _unitOfWork.EventRepository.DeleteById(id, cancellationToken);
                await _unitOfWork.EventRepository.Commit(cancellationToken);
            }
        }

        public async override Task<EventDto> Update(EventDto dto, CancellationToken cancellationToken)
        {
            var role = await _userService.GetUserRole(dto.Id, cancellationToken);
            var eventEntity = _mapper.Map<Event>(dto);
            if (dto.UserCreatorId == dto.Id || role.Role == "Admin")
            {
                await _unitOfWork.EventRepository.Update(eventEntity, cancellationToken);
                await _unitOfWork.EventRepository.Commit(cancellationToken);
                return dto;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async override Task<EventDto> Create(EventDto dto, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.FindBy(s => s.Name == dto.CategoryName)
                .FirstOrDefaultAsync(cancellationToken);
            if (category != null)
            {
                var eventInfo = new Event()
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    DateAndTime = dto.DateAndTime,
                    CategoryId = category.Id,
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
            else
            {
                throw new KeyNotFoundException("userId not found");
            }
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

        public async Task<UserDto> GetByName(string name, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.EventRepository.GetByName(name, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<EventDto>?> GetEventsByCriteria(DateTime? date, string? address, string? categoryName,
            CancellationToken cancellationToken)
        {
            var categoryId = Guid.Empty;
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = await _unitOfWork.CategoryRepository.FindBy(s => s.Name == categoryName).FirstOrDefaultAsync(cancellationToken);
                categoryId = category.Id;
            }

            var events = await _unitOfWork.EventRepository.GetEventsByCriteria(date, address, categoryId, cancellationToken);
            return _mapper.Map<List<EventDto>>(events);
        }
    }
}
