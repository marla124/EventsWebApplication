using EventsWebApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface ICategoryRepository 
    {
        public Task<Category[]> GetCategories(CancellationToken cancellationToken);
    }
}
