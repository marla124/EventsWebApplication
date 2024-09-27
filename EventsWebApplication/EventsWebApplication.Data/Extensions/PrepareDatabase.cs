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

            return app;
        }
    }
}
