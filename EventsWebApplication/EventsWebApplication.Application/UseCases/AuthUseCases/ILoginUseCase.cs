using EventsWebApplication.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.UseCases.AuthUseCases
{
    public interface ILoginUseCase
    {
        public Task<string> Execute(string email, string userAgent, CancellationToken cancellationToken);
    }
}
