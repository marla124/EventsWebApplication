using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.TokenUseCases.Interface
{
    public interface IGenerateJwtTokenUseCase
    {
        public Task<string> Execute(UserDto userDto, CancellationToken cancellationToken);

    }
}
