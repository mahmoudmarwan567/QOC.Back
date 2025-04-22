using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
using QOC.Domain.Entities;
using QOC.Infrastructure.Helpers;
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
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                AddressesAR = company.Addresses.Select(a => a.AddressAR).ToList(),
                AddressesEN = company.Addresses.Select(a => a.AddressEN).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = CultureHelper.IsArabic() ? cs.SocialNameAR : cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList()
            }).ToList();

            return companyDtos;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyRequestDto dto)
        {
            var company = new Company
            {
                NameAR = dto.NameAR,
                NameEN = dto.NameEN,
                Logo = dto.LogoPath,
                Addresses = dto.AddressesEN.Select((addrEN, index) => new CompanyAddress
                {
                    AddressEN = addrEN,
                    AddressAR = dto.AddressesAR.ElementAtOrDefault(index) // Match by index
                }).ToList(),
                Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList(),
                Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList(),
                CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
                {
                    SocialNameAR = cs.NameAR,
                    SocialNameEN = cs.NameEN,
                    SocialIconPath = cs.IconPath
                }).ToList()
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();


            return new CompanyDto
            {
                Id = company.Id,
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                AddressesAR = company.Addresses.Select(a => a.AddressAR).ToList(),
                AddressesEN = company.Addresses.Select(a => a.AddressEN).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = CultureHelper.IsArabic() ? cs.SocialNameAR : cs.SocialNameEN,
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
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                AddressesAR = company.Addresses.Select(a => a.AddressAR).ToList(),
                AddressesEN = company.Addresses.Select(a => a.AddressEN).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = CultureHelper.IsArabic() ? cs.SocialNameAR : cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList()
            };
        }

        // Update an existing company with manual mapping
        public async Task<CompanyDto> UpdateCompanyAsync(int id, CompanyRequestDto dto)
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
            company.NameAR = dto.NameAR;
            company.NameEN = dto.NameEN;
            company.Logo = dto.LogoPath;

            company.Addresses = dto.AddressesEN.Select((addrEN, index) => new CompanyAddress
            {
                AddressEN = addrEN,
                AddressAR = dto.AddressesAR.ElementAtOrDefault(index) // Match by index
            }).ToList();
            company.Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList();
            company.Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList();
            company.CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
            {
                SocialNameAR = cs.NameAR,
                SocialNameEN = cs.NameEN,
                SocialIconPath = cs.IconPath
            }).ToList();

            await _context.SaveChangesAsync();

            // Return updated entity as DTO
            return new CompanyDto
            {
                Id = company.Id,
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                AddressesAR = company.Addresses.Select(a => a.AddressAR).ToList(),
                AddressesEN = company.Addresses.Select(a => a.AddressEN).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    Name = CultureHelper.IsArabic() ? cs.SocialNameAR : cs.SocialNameEN,
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
