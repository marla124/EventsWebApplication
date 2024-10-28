using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class UploadImageUseCase : IUploadImageUseCase
    {
        private readonly IMapper _mapper;
        private readonly IGetEventByIdUseCase _getEventByIdUseCase;
        public UploadImageUseCase(IMapper mapper, IGetEventByIdUseCase getEventByIdUseCase)
        {
            _mapper = mapper;
            _getEventByIdUseCase = getEventByIdUseCase;
        }

        public async Task Execute(Guid eventId, Guid userId, IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException();
            }

            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var eventToUpdate = _mapper.Map<UpdateEventDto>(await _getEventByIdUseCase.Execute(eventId, cancellationToken));
            if (eventToUpdate == null)
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
