using AutoMapper;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using Moq;

namespace EventWebApplication.Tests
{
    public class EventUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IDeleteEventByIdUseCase _deleteEventByIdUseCase;
        private readonly IGetEventByNameUseCase _getEventByNameUseCase;
        private readonly Mock<IGetUserRoleUseCase> _getUserRoleUseCaseMock;

        public EventUseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _getEventByNameUseCase = new GetEventByNameUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _deleteEventByIdUseCase = new DeleteEventByIdUseCase(_unitOfWorkMock.Object);
            _getUserRoleUseCaseMock = new Mock<IGetUserRoleUseCase>();
        }

        [Fact]
        public async Task GetEventByName_ReturnEventDto()
        {
            var name = "event name";
            var eventInfo = new Event { Name = name };
            var eventDto = new EventDto { Name = name };

            _unitOfWorkMock.Setup(u => u.EventRepository.GetByName(name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventInfo);
            _mapperMock.Setup(m => m.Map<EventDto>(eventInfo)).Returns(eventDto);

            var result = await _getEventByNameUseCase.Execute(name, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task Execute_EventFound_DeletesEvent()
        {
            var eventId = Guid.NewGuid();
            var eventEntity = new Event { Id = eventId, Name = "Test Event" };

            _unitOfWorkMock.Setup(u => u.EventRepository.GetById(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity);

            await _deleteEventByIdUseCase.Execute(eventId, CancellationToken.None);

            _unitOfWorkMock.Verify(u => u.EventRepository.DeleteById(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.EventRepository.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Execute_EventNotFound_ThrowsKeyNotFoundException()
        {
            var eventId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.EventRepository.GetById(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _deleteEventByIdUseCase.Execute(eventId, CancellationToken.None));
        }
    }
}
