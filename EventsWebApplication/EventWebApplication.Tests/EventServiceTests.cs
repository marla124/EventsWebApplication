using AutoMapper;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using NSubstitute;
using Xunit;

namespace EventWebApplication.Tests
{
    public class EventServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapperMock;
        private readonly IDeleteEventByIdUseCase _deleteEventByIdUseCase;
        private readonly IGetEventByNameUseCase _getEventByNameUseCase;
        private readonly IGetUserRoleUseCase _getUserRoleUseCaseMock;

        public EventServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
            _getEventByNameUseCase = new GetEventByNameUseCase(_mapperMock, _unitOfWorkMock);
            _deleteEventByIdUseCase = new DeleteEventByIdUseCase(_unitOfWorkMock);
            _getUserRoleUseCaseMock = Substitute.For<IGetUserRoleUseCase>();
        }

        [Fact]
        public async Task GetEventByName_ReturnEventDto()
        {
            var name = "event name";
            var eventInfo = new Event { Name = name };
            var eventDto = new EventDto { Name = name };

            _unitOfWorkMock.EventRepository.GetByName(name, CancellationToken.None).Returns(Task.FromResult(eventInfo));
            _mapperMock.Map<EventDto>(eventInfo).Returns(eventDto);

            var result = await _getEventByNameUseCase.Execute(name, CancellationToken.None);

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
            _getUserRoleUseCaseMock.Execute(userId, CancellationToken.None)
                .Returns(Task.FromResult(userRole));
            _unitOfWorkMock.EventRepository.DeleteById(eventId, CancellationToken.None)
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.EventRepository.Commit(CancellationToken.None)
                .Returns(Task.CompletedTask);

            await _deleteEventByIdUseCase.Execute(eventId, userId, CancellationToken.None);

            await _unitOfWorkMock.EventRepository.Received(1).DeleteById(eventId, CancellationToken.None);
            await _unitOfWorkMock.EventRepository.Received(1).Commit(CancellationToken.None);
        }
    }
}
