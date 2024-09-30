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
    }
}
