using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventsWebApplication.Data.Extensions
{
    public static class AppBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
            var dbContext = serviceProvider.GetRequiredService<EventWebApplicationDbContext>();

            if (!await dbContext.UserRoles.AnyAsync())
            {
                var roles = new List<UserRole>
                {
                    new UserRole { Role = "Admin" },
                    new UserRole { Role = "User" }
                };

                await dbContext.UserRoles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }
            var adminRole = await dbContext.UserRoles.FirstOrDefaultAsync(r => r.Role == "Admin");
            var userRole = await dbContext.UserRoles.FirstOrDefaultAsync(r => r.Role == "User");

            if (adminRole != null && !await dbContext.Users.AnyAsync())
            {
                var userAdmin = new User()
                {
                    Name = "Admin",
                    Surname = "User",
                    Email = "admin@example.com",
                    PasswordHash = "6C53D1FC07B1AEA35563A0682987E6A1", //string12345678
                    UserRoleId = adminRole.Id,
                };
                var userAdmin1 = new User()
                {
                    Name = "Admin",
                    Surname = "User",
                    Email = "userU@example.com",
                    PasswordHash = "6C53D1FC07B1AEA35563A0682987E6A1", //string12345678
                    UserRoleId = adminRole.Id,
                };
                var userU = new User()
                {
                    Name = "Admin",
                    Surname = "User",
                    Email = "admin11111@example.com",
                    PasswordHash = "6C53D1FC07B1AEA35563A0682987E6A1", //string12345678
                    UserRoleId = adminRole.Id,
                };
                await dbContext.Users.AddAsync(userAdmin1);
                await dbContext.Users.AddAsync(userU);
                await dbContext.Users.AddAsync(userAdmin);
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Business Events" },
                    new Category { Name = "Entertainment Events" },
                    new Category { Name = "Cultural Events" },
                    new Category { Name = "Sports Events" },
                    new Category { Name = "Charity Events" },
                    new Category { Name = "Other Events" }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }

            return app;
        }
    }
}
