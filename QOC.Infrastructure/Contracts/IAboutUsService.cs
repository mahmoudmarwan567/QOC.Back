using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QOC.Application.DTOs;
using QOC.Domain.Entities;

namespace QOC.Application.Interfaces
{
    public interface IAboutUsService
    {
        Task<IEnumerable<AboutUs>> GetAllAsync();
        Task<AboutUs> GetByIdAsync(int id);
        Task<AboutUs> CreateAsync(AboutUsDto dto);
        Task<AboutUs> UpdateAsync(int id, AboutUsDto dto);
        Task DeleteAsync(int id);
    }
}
