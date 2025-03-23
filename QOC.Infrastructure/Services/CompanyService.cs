using QOC.Domain.Entities;
using QOC.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOC.Application.DTOs;    // المسار للـ DTOs
using QOC.Domain.Entities;

using Microsoft.EntityFrameworkCore;



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
                                 .ToListAsync();
        }
        public async Task<Company> CreateCompanyAsync(CompanyDto dto)
        {
            // تحويل DTO -> Entity
            var company = new Company
            {
                Name = dto.Name,
                Logo = dto.Logo,
                // تحويل القوائم (Addresses, Phones, Emails) إلى كيانات فرعية
                Addresses = dto.Addresses.Select(a => new CompanyAddress { Address = a }).ToList(),
                Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList(),
                Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList()
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

            // تعديل البيانات الأساسية
            company.Name = dto.Name;
            company.Logo = dto.Logo;

            // حذف القوائم القديمة وإضافة الجديدة (أو إجراء منطق أكثر دقة)
            company.Addresses.Clear();
            company.Addresses = dto.Addresses.Select(a => new CompanyAddress { Address = a }).ToList();

            company.Phones.Clear();
            company.Phones = dto.Phones.Select(p => new CompanyPhone { PhoneNumber = p }).ToList();

            company.Emails.Clear();
            company.Emails = dto.Emails.Select(e => new CompanyEmail { Email = e }).ToList();

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
