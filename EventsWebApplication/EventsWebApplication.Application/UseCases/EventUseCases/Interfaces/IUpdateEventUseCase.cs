using EventsWebApplication.Application.Dto;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IUpdateEventUseCase
{
    public Task<UpdateEventDto> Execute(UpdateEventDto dto, Guid userId, IFormFile? file, CancellationToken cancellationToken);

}