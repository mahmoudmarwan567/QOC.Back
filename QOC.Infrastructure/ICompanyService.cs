using QOC.Application.DTOs;
using QOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOC.Infrastructure.Services
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(CompanyDto dto);
        Task<Company> GetCompanyByIdAsync(int id);
        Task<Company> UpdateCompanyAsync(int id, CompanyDto dto);
        Task DeleteCompanyAsync(int id);
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        // ... أي عمليات أخرى مطلوبة
    }

}
