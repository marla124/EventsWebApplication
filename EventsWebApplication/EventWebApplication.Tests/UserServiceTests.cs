using AutoMapper;
using EventsWebApplication.BL;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using EventsWebApplication.BL.Interfaces;
using Xunit;
using System.Linq.Expressions;
using EventsWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EventWebApplication.Tests
{
    public class UserServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IConfiguration _configurationMock;
        private readonly IMapper _mapperMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _configurationMock = Substitute.For<IConfiguration>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
            _userService = new UserService(_mapperMock, _unitOfWorkMock, _configurationMock);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnUserDto()
        {
            var email = "useremail@gmail.com";
            var user = new User { Email = email };
            var userDto = new UserDto { Email = email };

            _unitOfWorkMock.UserRepository.GetByEmail(email, CancellationToken.None).Returns(Task.FromResult(user));
            _mapperMock.Map<UserDto>(user).Returns(userDto);

            var result = await _userService.GetByEmail(email, CancellationToken.None);

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

            var result = await _userService.GetById(id, CancellationToken.None);

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

            var result = await _userService.RegisterUser(userDto, CancellationToken.None);

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

            var result = await _userService.GetUserByRefreshToken(id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Contains(refToken, user.RefreshTokens);
        }
    }
}
