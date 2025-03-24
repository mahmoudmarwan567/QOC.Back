using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;    // المسار للـ DTOs
using QOC.Domain.Entities;
using QOC.Infrastructure.Persistence;



namespace QOC.Infrastructure.Services
{


    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public CompanyService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                                 .Include(c => c.Addresses)
                                 .Include(c => c.Phones)
                                 .Include(c => c.Emails)
                                 .ToListAsync();
        }
        public async Task<Company> CreateCompanyAsync(CompanyDto dto)
        {
            if (dto.LogoFile == null || dto.LogoFile.Length == 0)
                throw new ArgumentException("لم يتم اختيار صورة");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets/images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.LogoFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.LogoFile.CopyToAsync(stream);
            }

            var logoPath = $"/assets/images/{fileName}";

            var company = new Company
            {
                Name = dto.Name,
                Logo = logoPath,
                Addresses = dto.Addresses?.Select(a => new CompanyAddress { Address = a }).ToList() ?? new List<CompanyAddress>(),
                Phones = dto.Phones?.Select(p => new CompanyPhone { PhoneNumber = p }).ToList() ?? new List<CompanyPhone>(),
                Emails = dto.Emails?.Select(e => new CompanyEmail { Email = e }).ToList() ?? new List<CompanyEmail>()
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
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var company = await _context.Companies
        .Include(c => c.Addresses)
        .Include(c => c.Phones)
        .Include(c => c.Emails)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return null;

            company.Name = dto.Name;

            if (dto.LogoFile != null && dto.LogoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets/images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(company.Logo))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, company.Logo.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.LogoFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.LogoFile.CopyToAsync(stream);
                }
                company.Logo = $"/assets/images/{fileName}";
            }

            company.Addresses.Clear();
            company.Addresses = dto.Addresses?.Select(a => new CompanyAddress { Address = a }).ToList() ?? new List<CompanyAddress>();

            company.Phones.Clear();
            company.Phones = dto.Phones?.Select(p => new CompanyPhone { PhoneNumber = p }).ToList() ?? new List<CompanyPhone>();

            company.Emails.Clear();
            company.Emails = dto.Emails?.Select(e => new CompanyEmail { Email = e }).ToList() ?? new List<CompanyEmail>();

            await _context.SaveChangesAsync();
            return company;
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }

}
