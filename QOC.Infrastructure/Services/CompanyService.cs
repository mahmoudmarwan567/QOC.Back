using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;    // المسار للـ DTOs
using QOC.Domain.Entities;
using QOC.Infrastructure.Persistence;



namespace QOC.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CompanyService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _context.Companies
            .AsNoTracking()
            .Include(c => c.Addresses)
            .Include(c => c.Emails)
            .Include(c => c.Phones)
            .Include(c => c.CompanySocials)
            .ToListAsync();

            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto)
        {
            var company = _mapper.Map<Company>(dto);

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
            .Include(c => c.Addresses)
            .Include(c => c.Phones)
            .Include(c => c.Emails)
            .Include(c => c.CompanySocials)
            .FirstOrDefaultAsync(c => c.Id == id);

            return company == null ? null : _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var company = await _context.Companies
            .Include(c => c.Addresses)
            .Include(c => c.Phones)
            .Include(c => c.Emails)
            .Include(c => c.CompanySocials)
            .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            // Optional: Clear related collections if needed
            company.Addresses.Clear();
            company.Phones.Clear();
            company.Emails.Clear();
            company.CompanySocials.Clear();

            // Map updated fields from DTO to the entity
            _mapper.Map(dto, company);

            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.CompanySocials)
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }

}
