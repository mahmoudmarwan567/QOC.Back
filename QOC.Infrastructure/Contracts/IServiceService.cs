using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QOC.Application.DTOs;
using QOC.Domain.Entities;

namespace QOC.Application.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(int id);
        Task<Service> CreateAsync(ServiceDto dto);
        Task<Service> UpdateAsync(int id, ServiceDto dto);
        Task DeleteAsync(int id);
    }
}

