using System.IdentityModel.Tokens.Jwt;
using EventsWebApplication.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using EventsWebApplication.Data.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EventWebApplication.IntegrationTests
{
    public class BaseIntegrationTest : IDisposable
    {
        private readonly EventWebApplicationDbContext? _eventDbContext;

        protected readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        public BaseIntegrationTest()
        {
            _webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptorEvent = services.SingleOrDefault(d => d.ServiceType ==
                            typeof(DbContextOptions<EventWebApplicationDbContext>));
                    if (descriptorEvent != null)
                        services.Remove(descriptorEvent);
                    services.AddDbContext<EventWebApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryEventTest");
                    });
                });
            });

            var serviceProvider = _webApplicationFactory.Services.CreateScope().ServiceProvider;

            _httpClient = _webApplicationFactory.CreateClient();
            var fakeUserJwt = CreateJwtForFakeUser();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fakeUserJwt);
            _eventDbContext = serviceProvider.GetService<EventWebApplicationDbContext>()!;
            _eventDbContext?.Database.EnsureCreated();
        }

        public static string CreateJwtForFakeUser()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EB622E6F-F21D-44ED-9798-D993A8126605"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "UserTest"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", "FD596604-0708-4841-A4CC-C0E8B3F87B5F"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var token = new JwtSecurityToken(
                issuer: "EventWebApplication",
                audience: "EventWebApplication",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine("Generated Token: " + tokenString);

            return tokenString;
        }

        protected async Task<RefreshToken> PopulateTokenToDatabase()
        {
            var user = await PopulateUserToDatabase();
            var token = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                ExpiringAt = DateTime.Now,
                AssociateDeviceName = "name",
                UserId = user.Id,
            };

            _eventDbContext!.RefreshTokens.Add(token);
            await _eventDbContext.SaveChangesAsync();

            return token;
        }
        protected async Task<User> PopulateUserToDatabase()
        {
            var user = new User
            {
                Name = "user",
                Surname = "user",
                Email = "testemail2@gmail.com",
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                UpdatedAt = DateTime.Now,
                PasswordHash = "5F4DCC3B5AA765D61D8327DEB882CF99",
                UserRoleId = Guid.Parse("7f37b740-8a9b-45b2-8871-c3dba40fa673")
            };

            var userRole = new UserRole
            {
                Id = Guid.Parse("7f37b740-8a9b-45b2-8871-c3dba40fa673"),
                Role = "Admin"
            };
            _eventDbContext!.UserRoles.Add(userRole);
            _eventDbContext!.Users.Add(user);
            await _eventDbContext.SaveChangesAsync();

            return user;
        }

        protected async Task<Category> PopulateCategoryToDatabase()
        {
            var category = new Category
            {
                Name = "category",
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                UpdatedAt = DateTime.Now,

            };

            _eventDbContext!.Categories.Add(category);
            await _eventDbContext.SaveChangesAsync();

            return category;
        }

        public void Dispose()
        {
            _eventDbContext?.Database.EnsureDeleted();
            _eventDbContext?.Dispose();
            _httpClient.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
