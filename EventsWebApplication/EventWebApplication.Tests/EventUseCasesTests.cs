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
    public class EventUseCasesTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapperMock;
        private readonly IDeleteEventByIdUseCase _deleteEventByIdUseCase;
        private readonly IGetEventByNameUseCase _getEventByNameUseCase;
        private readonly IGetUserRoleUseCase _getUserRoleUseCaseMock;

        public EventUseCasesTests()
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
        public async Task Execute_EventFound_DeletesEvent()
        {
            var eventId = Guid.NewGuid();
            var eventEntity = new Event { Id = eventId, Name = "Test Event" };

            _unitOfWorkMock.EventRepository.GetById(eventId, CancellationToken.None)
                .Returns(Task.FromResult(eventEntity));

            await _deleteEventByIdUseCase.Execute(eventId, CancellationToken.None);

            await _unitOfWorkMock.EventRepository.Received(1).DeleteById(eventEntity, CancellationToken.None);
            await _unitOfWorkMock.EventRepository.Received(1).Commit(CancellationToken.None);
        }

        [Fact]
        public async Task Execute_EventNotFound_ThrowsKeyNotFoundException()
        {
            var eventId = Guid.NewGuid();

            _unitOfWorkMock.EventRepository.GetById(eventId, CancellationToken.None)
                .Returns(Task.FromResult<Event>(null));

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _deleteEventByIdUseCase.Execute(eventId, CancellationToken.None));
        }
    }
}
