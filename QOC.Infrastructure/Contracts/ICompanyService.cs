using QOC.Application.DTOs;

namespace QOC.Infrastructure.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateCompanyAsync(CompanyRequestDto dto);
        Task<CompanyDto> GetCompanyByIdAsync(int id);
        Task<CompanyDto> UpdateCompanyAsync(int id, CompanyRequestDto dto);
        Task DeleteCompanyAsync(int id);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        // ... أي عمليات أخرى مطلوبة
    }

}
