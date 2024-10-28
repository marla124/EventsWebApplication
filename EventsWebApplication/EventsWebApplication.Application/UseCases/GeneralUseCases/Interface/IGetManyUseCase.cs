namespace EventsWebApplication.Application.UseCases.GeneralUseCases.Interface
{
    public interface IGetManyUseCase<TDto>
    {
        public Task<TDto[]?> Execute(CancellationToken cancellationToken);
    }
}
