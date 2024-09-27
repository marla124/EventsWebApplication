using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.BL.Interfaces
{
    public interface IService<TDto>
    {
        public Task<TDto?> Create(TDto dto, CancellationToken cancellationToken);
        public Task DeleteById(Guid id, CancellationToken cancellationToken);
        public Task<TDto?> GetById(Guid Id, CancellationToken cancellationToken);
        public Task<TDto[]?> GetMany(CancellationToken cancellationToken);
        public Task<TDto?> Update(TDto dto, CancellationToken cancellationToken);
    }
}
