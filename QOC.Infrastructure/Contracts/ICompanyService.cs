using QOC.Application.DTOs;

namespace QOC.Infrastructure.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateCompanyAsync(CompanyDto dto);
        Task<CompanyDto> GetCompanyByIdAsync(int id);
        Task<CompanyDto> UpdateCompanyAsync(int id, CompanyDto dto);
        Task DeleteCompanyAsync(int id);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        // ... أي عمليات أخرى مطلوبة
    }

}
