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

        // Get all companies with manual mapping
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _context.Companies
                .AsNoTracking()
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.CompanySocials)
                .ToListAsync();

            var companyDtos = companies.Select(company => new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                LogoPath = company.Logo, // Mapping Logo field
                Addresses = company.Addresses.Select(a => a.Address).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = cs.SocialName,
                    IconPath = cs.SocialIconPath
                }).ToList()
            }).ToList();

            return companyDtos;
        }

        // Create a new company and return as DTO with manual mapping
        public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                Logo = dto.LogoPath, // Mapping LogoPath in DTO to Logo in Entity
                Addresses = dto.Addresses.Select(a => new CompanyAddress { Address = a }).ToList(),
                Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList(),
                Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList(),
                CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
                {
                    SocialName = cs.Name,
                    SocialIconPath = cs.IconPath
                }).ToList()
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Return created entity as DTO
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => a.Address).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = cs.SocialName,
                    IconPath = cs.SocialIconPath
                }).ToList()
            };
        }

        // Get a single company by Id with manual mapping
        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.CompanySocials)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => a.Address).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = cs.SocialName,
                    IconPath = cs.SocialIconPath
                }).ToList()
            };
        }

        // Update an existing company with manual mapping
        public async Task<CompanyDto> UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var company = await _context.Companies
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .Include(c => c.Emails)
                .Include(c => c.CompanySocials)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            // Clear related collections before updating
            company.Addresses.Clear();
            company.Phones.Clear();
            company.Emails.Clear();
            company.CompanySocials.Clear();

            // Manually update fields
            company.Name = dto.Name;
            company.Logo = dto.LogoPath;

            company.Addresses = dto.Addresses.Select(a => new CompanyAddress { Address = a }).ToList();
            company.Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList();
            company.Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList();
            company.CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
            {
                SocialName = cs.Name,
                SocialIconPath = cs.IconPath
            }).ToList();

            await _context.SaveChangesAsync();

            // Return updated entity as DTO
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => a.Address).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = cs.SocialName,
                    IconPath = cs.SocialIconPath
                }).ToList()
            };
        }

        // Delete a company
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
