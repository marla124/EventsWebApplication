using System.Linq.Expressions;
using AutoMapper;
using Castle.Core.Configuration;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventWebApplication.Tests
{
    public class UserServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IConfiguration _configurationMock;
        private readonly IMapper _mapperMock;
        private readonly IPasswordUseCase _passwordMock;
        private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly IGetUserByRefreshTokenUseCase _getUserByRefreshTokenUseCase;
        private readonly IRegisterUserUseCase _registerUserUseCase;

        public UserServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
            _passwordMock = Substitute.For<IPasswordUseCase>();
            _getUserByEmailUseCase = new GetUserByEmailUseCase(_mapperMock, _unitOfWorkMock);
            _getUserByIdUseCase = new GetUserByIdUseCase(_unitOfWorkMock.UserRepository, _mapperMock);
            _getUserByRefreshTokenUseCase = new GetUserByRefreshTokenUseCase(_mapperMock, _unitOfWorkMock);
            _registerUserUseCase = new RegisterUserUseCase(_unitOfWorkMock, _mapperMock, _passwordMock);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnUserDto()
        {
            var email = "useremail@gmail.com";
            var user = new User { Email = email };
            var userDto = new UserDto { Email = email };

            _unitOfWorkMock.UserRepository.GetByEmail(email, CancellationToken.None).Returns(Task.FromResult(user));
            _mapperMock.Map<UserDto>(user).Returns(userDto);

            var result = await _getUserByEmailUseCase.Execute(email, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.Email, email);
        }

        [Fact]
        public async Task GetUserById_ReturnUserDto()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id };
            var userDto = new UserDto { Id = id };

            _unitOfWorkMock.UserRepository.GetById(id, CancellationToken.None).Returns(Task.FromResult(user));
            _mapperMock.Map<UserDto>(user).Returns(userDto);

            var result = await _getUserByIdUseCase.Execute(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.Id, id);
        }

        [Fact]
        public async Task RegisterUser_SuccessCreatedAndReturnDto()
        {
            var options = new DbContextOptionsBuilder<EventWebApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new EventWebApplicationDbContext(options);
            var userDto = new UserDto { Id = Guid.NewGuid()};
            var userRole = new UserRole { Id = Guid.NewGuid(), Role = "User" };
            context.UserRoles.Add(userRole);
            context.SaveChanges();

            var user = new User
            {
                Id = userDto.Id,
                UserRole = userRole
            };

            _unitOfWorkMock.UserRoleRepository.FindBy(Arg.Any<Expression<Func<UserRole, bool>>>())
                .Returns(context.UserRoles.AsQueryable());
            _unitOfWorkMock.UserRepository.CreateOne(Arg.Any<User>(), CancellationToken.None).Returns(user);
            _unitOfWorkMock.UserRepository.Commit(CancellationToken.None).Returns(Task.CompletedTask);
            _mapperMock.Map<UserDto>(Arg.Any<User>()).Returns(userDto);

            var result = await _registerUserUseCase.Execute(userDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userDto.Id, result.Id);

        }

        [Fact]
        public async Task GetUserByRefreshToken_ReturnUserDto()
        {
            var userDto = new UserDto();
            var id = Guid.NewGuid();
            var refToken = new RefreshToken(){Id =id};
            var user = new User
            {
                RefreshTokens = new List<RefreshToken>()
            };
            user.RefreshTokens.Add(refToken);


            _unitOfWorkMock.UserRepository.GetByRefreshToken(id, CancellationToken.None)
                .Returns(Task.FromResult(user));
            _mapperMock.Map<UserDto>(user).Returns(userDto);

            var result = await _getUserByRefreshTokenUseCase.Execute(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Contains(refToken, user.RefreshTokens);
        }
    }
}
