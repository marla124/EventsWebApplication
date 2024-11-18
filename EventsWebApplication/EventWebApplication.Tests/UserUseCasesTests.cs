using System.Linq.Expressions;
using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.AuthUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EventWebApplication.Tests
{
    public class UserUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordService> _passwordMock;
        private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly IGetUserByRefreshTokenUseCase _getUserByRefreshTokenUseCase;
        private readonly IRegisterUserUseCase _registerUserUseCase;

        public UserUseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _passwordMock = new Mock<IPasswordService>();
            _getUserByEmailUseCase = new GetUserByEmailUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getUserByIdUseCase = new GetUserByIdUseCase(_mapperMock.Object, _unitOfWorkMock.Object);
            _getUserByRefreshTokenUseCase = new GetUserByRefreshTokenUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _registerUserUseCase = new RegisterUserUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _passwordMock.Object);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnUserDto()
        {
            var email = "useremail@gmail.com";
            var user = new User { Email = email };
            var userDto = new UserDto { Email = email };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByEmail(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _getUserByEmailUseCase.Execute(email, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetUserById_ReturnUserDto()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id };
            var userDto = new UserDto { Id = id };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetById(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _getUserByIdUseCase.Execute(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task RegisterUser_SuccessCreatedAndReturnDto()
        {
            var options = new DbContextOptionsBuilder<EventWebApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new EventWebApplicationDbContext(options);
            var userDto = new UserDto { Id = Guid.NewGuid() };
            var userRole = new UserRole { Id = Guid.NewGuid(), Role = "User" };
            context.UserRoles.Add(userRole);
            context.SaveChanges();

            var user = new User
            {
                Id = userDto.Id,
                UserRole = userRole
            };

            _unitOfWorkMock.Setup(u => u.UserRoleRepository.FindBy(It.IsAny<Expression<Func<UserRole, bool>>>()))
                .Returns(context.UserRoles.AsQueryable());
            _unitOfWorkMock.Setup(u => u.UserRepository.CreateOne(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.UserRepository.Commit(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var result = await _registerUserUseCase.Execute(userDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userDto.Id, result.Id);
        }

        [Fact]
        public async Task GetUserByRefreshToken_ReturnUserDto()
        {
            var userDto = new UserDto();
            var id = Guid.NewGuid();
            var refToken = new RefreshToken() { Id = id };
            var user = new User
            {
                RefreshTokens = new List<RefreshToken> { refToken }
            };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByRefreshToken(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _getUserByRefreshTokenUseCase.Execute(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Contains(refToken, user.RefreshTokens);
        }
    }
}
