using AutoMapper;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.BL;
using EventsWebApplication.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace EventWebApplication.Tests
{
    public class EventServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapperMock;
        private readonly IUserService _userServiceMock;
        private readonly IEventService _eventService;


        public EventServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
            _userServiceMock = Substitute.For<IUserService>();
            _eventService = new EventService(_mapperMock, _userServiceMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task GetEventByName_ReturnEventDto()
        {
            var name = "event name";
            var eventInfo = new Event { Name = name };
            var eventDto = new EventDto { Name = name };

            _unitOfWorkMock.EventRepository.GetByName(name, CancellationToken.None).Returns(Task.FromResult(eventInfo));
            _mapperMock.Map<EventDto>(eventInfo).Returns(eventDto);

            var result = await _eventService.GetByName(name, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.Name, name);
        }

        [Fact]
        public async Task DeleteEventByIdAdmin_CheckExistEvent()
        {
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var userRole = new UserRoleDto { Role = "Admin" };
            var eventInfo = new Event { Id = eventId, Name = "event" };

            await GeneralyLogicDelete(eventId, userId, userRole, eventInfo);
        }

        [Fact]
        public async Task DeleteEventByIdCreatorUser_CheckExistEvent()
        {
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var userRole = new UserRoleDto { Role = "User" };
            var eventDto = new EventDto { Id = eventId, UserCreatorId = userId, Name = "event" };
            var eventInfo = new Event { Id = eventId, UserCreatorId = userId, Name = "event" };

            _mapperMock.Map<EventDto>(eventInfo).Returns(eventDto);

            await GeneralyLogicDelete(eventId, userId, userRole, eventInfo);
        }

        private async Task GeneralyLogicDelete(Guid eventId, Guid userId, UserRoleDto userRole, Event eventInfo)
        {
            _unitOfWorkMock.EventRepository.GetById(eventId, CancellationToken.None)
                .Returns(Task.FromResult(eventInfo));
            _userServiceMock.GetUserRole(userId, CancellationToken.None)
                .Returns(Task.FromResult(userRole));
            _unitOfWorkMock.EventRepository.DeleteById(eventId, CancellationToken.None)
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.EventRepository.Commit(CancellationToken.None)
                .Returns(Task.CompletedTask);

            await _eventService.DeleteById(eventId, userId, CancellationToken.None);

            await _unitOfWorkMock.EventRepository.Received(1).DeleteById(eventId, CancellationToken.None);
            await _unitOfWorkMock.EventRepository.Received(1).Commit(CancellationToken.None);
        }
    }
}

