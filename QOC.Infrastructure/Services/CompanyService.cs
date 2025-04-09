using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;    // المسار للـ DTOs
using QOC.Domain.Entities;
using QOC.Infrastructure.Persistence;



namespace QOC.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                                 .Include(c => c.Addresses)
                                 .Include(c => c.Phones)
                                 .Include(c => c.Emails)
                                 .Include(c => c.CompanySocials)
                                 .ToListAsync();
        }

        public async Task<Company> CreateCompanyAsync(CompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                Logo = dto.LogoPath,
                Addresses = dto.Addresses?.Select(a => new CompanyAddress { Address = a }).ToList() ?? new List<CompanyAddress>(),
                Phones = dto.Phones?.Select(p => new CompanyPhone { PhoneNumber = p }).ToList() ?? new List<CompanyPhone>(),
                Emails = dto.Emails?.Select(e => new CompanyEmail { Email = e }).ToList() ?? new List<CompanyEmail>(),
                CompanySocials = dto.CompanySocials?.Select(s => new CompanySocial
                {
                    SocialName = s.Name,
                    SocialIconPath = s.IconPath
                }).ToList() ?? new List<CompanySocial>()
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.CompanySocials)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var company = await _context.Companies
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.CompanySocials) // NEW
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            company.Name = dto.Name;
            company.Logo = dto.LogoPath;

            company.Addresses.Clear();
            company.Addresses = dto.Addresses?.Select(a => new CompanyAddress { Address = a }).ToList() ?? new List<CompanyAddress>();

            company.Phones.Clear();
            company.Phones = dto.Phones?.Select(p => new CompanyPhone { PhoneNumber = p }).ToList() ?? new List<CompanyPhone>();

            company.Emails.Clear();
            company.Emails = dto.Emails?.Select(e => new CompanyEmail { Email = e }).ToList() ?? new List<CompanyEmail>();

            company.CompanySocials.Clear();
            company.CompanySocials = dto.CompanySocials?.Select(s => new CompanySocial
            {
                SocialName = s.Name,
                SocialIconPath = s.IconPath
            }).ToList() ?? new List<CompanySocial>();

            await _context.SaveChangesAsync();
            return company;
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
