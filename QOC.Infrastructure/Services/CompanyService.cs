using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
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
                .Include(c => c.Documents)
                .ToListAsync();

            var companyDtos = companies.Select(company => new CompanyDto
            {
                Id = company.Id,
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => new CompanyAddressDto
                {
                    AddressAR = a.AddressAR,
                    AddressEN = a.AddressEN,
                    MapLink = a.MapLink
                }).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    NameAR = cs.SocialNameAR,
                    NameEN = cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList(),
                Documents = company.Documents.Select(d => new CompanyDocumentDto
                {
                    Id = d.Id,
                    TitleAR = d.TitleAR,
                    TitleEN = d.TitleEN,
                    FilePath = d.FilePath,
                    Description = d.Description,
                    UploadedDate = d.UploadedDate
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
                Addresses = dto.Addresses.Select(addr => new CompanyAddress
                {
                    AddressAR = addr.AddressAR,
                    AddressEN = addr.AddressEN,
                    MapLink = addr.MapLink
                }).ToList(),
                Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList(),
                Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList(),
                CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
                {
                    SocialNameAR = cs.NameAR,
                    SocialNameEN = cs.NameEN,
                    SocialIconPath = cs.IconPath
                }).ToList(),
                Documents = dto.Documents.Select(d => new CompanyDocument
                {
                    TitleAR = d.TitleAR,
                    TitleEN = d.TitleEN,
                    FilePath = d.FilePath,
                    Description = d.Description,
                    UploadedDate = DateTime.UtcNow
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
                Addresses = company.Addresses.Select(a => new CompanyAddressDto
                {
                    AddressAR = a.AddressAR,
                    AddressEN = a.AddressEN,
                    MapLink = a.MapLink
                }).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    NameAR = cs.SocialNameAR,
                    NameEN = cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList(),
                Documents = company.Documents.Select(d => new CompanyDocumentDto
                {
                    Id = d.Id,
                    TitleAR = d.TitleAR,
                    TitleEN = d.TitleEN,
                    FilePath = d.FilePath,
                    Description = d.Description,
                    UploadedDate = d.UploadedDate
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
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            return new CompanyDto
            {
                Id = company.Id,
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => new CompanyAddressDto
                {
                    AddressAR = a.AddressAR,
                    AddressEN = a.AddressEN,
                    MapLink = a.MapLink
                }).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    NameAR = cs.SocialNameAR,
                    NameEN = cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList(),
                Documents = company.Documents.Select(d => new CompanyDocumentDto
                {
                    Id = d.Id,
                    TitleAR = d.TitleAR,
                    TitleEN = d.TitleEN,
                    FilePath = d.FilePath,
                    Description = d.Description,
                    UploadedDate = d.UploadedDate
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
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            // Clear related collections before updating
            company.Addresses.Clear();
            company.Phones.Clear();
            company.Emails.Clear();
            company.CompanySocials.Clear();
            company.Documents.Clear();

            // Manually update fields
            company.NameAR = dto.NameAR;
            company.NameEN = dto.NameEN;
            company.Logo = dto.LogoPath;

            company.Addresses = dto.Addresses.Select(addr => new CompanyAddress
            {
                AddressAR = addr.AddressAR,
                AddressEN = addr.AddressEN,
                MapLink = addr.MapLink
            }).ToList();
            company.Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList();
            company.Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList();
            company.CompanySocials = dto.CompanySocials.Select(cs => new CompanySocial
            {
                SocialNameAR = cs.NameAR,
                SocialNameEN = cs.NameEN,
                SocialIconPath = cs.IconPath
            }).ToList();
            company.Documents = dto.Documents.Select(d => new CompanyDocument
            {
                TitleAR = d.TitleAR,
                TitleEN = d.TitleEN,
                FilePath = d.FilePath,
                Description = d.Description,
                UploadedDate = DateTime.UtcNow
            }).ToList();

            await _context.SaveChangesAsync();

            // Return updated entity as DTO
            return new CompanyDto
            {
                Id = company.Id,
                NameAR = company.NameAR,
                NameEN = company.NameEN,
                LogoPath = company.Logo,
                Addresses = company.Addresses.Select(a => new CompanyAddressDto
                {
                    AddressAR = a.AddressAR,
                    AddressEN = a.AddressEN,
                    MapLink = a.MapLink
                }).ToList(),
                Phones = company.Phones.Select(p => p.PhoneNumber).ToList(),
                Emails = company.Emails.Select(e => e.Email).ToList(),
                CompanySocials = company.CompanySocials.Select(cs => new CompanySocialDto
                {
                    NameAR = cs.SocialNameAR,
                    NameEN = cs.SocialNameEN,
                    IconPath = cs.SocialIconPath
                }).ToList(),
                Documents = company.Documents.Select(d => new CompanyDocumentDto
                {
                    Id = d.Id,
                    TitleAR = d.TitleAR,
                    TitleEN = d.TitleEN,
                    FilePath = d.FilePath,
                    Description = d.Description,
                    UploadedDate = d.UploadedDate
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
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
