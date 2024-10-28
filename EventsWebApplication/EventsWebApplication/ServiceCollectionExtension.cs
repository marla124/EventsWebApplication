using System.Text;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.CategoryUseCase;
using EventsWebApplication.Application.UseCases.EventUseCases;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Application.UseCases.GeneralUseCases;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;
using EventsWebApplication.Application.UseCases.ParticipantUseCases;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Application.UseCases.TokenUseCases;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Data.Repositories;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure;
using EventsWebApplication.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventsWebApplication
{
    internal static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<EventWebApplicationDbContext>(opt => opt.UseNpgsql(connectionString));
            services.AddFluentValidationAutoValidation();
            
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IPasswordUseCase, PasswordUseCase>();
            services.AddScoped<IGetUserRoleUseCase, GetUserRoleUseCase>();
            services.AddScoped<IGetUserByRefreshTokenUseCase, GetUserByRefreshTokenUseCase>();
            services.AddScoped<IGetUserByEmailUseCase, GetUserByEmailUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDeleteUserByIdUseCase, DeleteUserByIdUseCase>();
            services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
            services.AddScoped<IDeleteEventByIdUseCase, DeleteEventByIdUseCase>();
            services.AddScoped<IGetEventByNameUseCase, GetEventByNameUseCase>();
            services.AddScoped<IGetEventsByCriteriaUseCase, GetEventsByCriteriaUseCase>();
            services.AddScoped<IGetUsersEventsUseCase, GetUsersEventsUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<IUploadImageUseCase, UploadImageUseCase>();
            services.AddScoped<IAddParticipantToEventUseCase, AddParticipantToEventUseCase>();
            services.AddScoped<IDeleteParticipantFromEventUseCase, DeleteParticipantFromEventUseCase>();
            services.AddScoped<IGetEventParticipantByIdUseCase, GetEventParticipantByIdUseCase>();
            services.AddScoped<IGetEventParticipantsUseCase, GetEventParticipantsUseCase>();
            services.AddScoped<IAddRefreshTokenUseCase, AddRefreshTokenUseCase>();
            services.AddScoped<ICheckRefreshTokenUseCase, CheckRefreshTokenUseCase>();
            services.AddScoped<IGenerateJwtTokenUseCase, GenerateJwtTokenUseCase>();
            services.AddScoped<IRemoveRefreshTokenUseCase, RemoveRefreshTokenUseCase>();
            services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IGetEventsUseCase, GetEventsUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var secret = configuration["Jwt:Secret"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
        }
    }
}
