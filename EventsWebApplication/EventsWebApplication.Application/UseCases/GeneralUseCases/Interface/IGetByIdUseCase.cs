namespace EventsWebApplication.Application.UseCases.GeneralUseCases.Interface
{
    public interface IGetByIdUseCase<TDto>
    {
        public Task<TDto?> Execute(Guid Id, CancellationToken cancellationToken);
    }
}
