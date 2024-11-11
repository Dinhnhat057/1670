using Microsoft.EntityFrameworkCore;
using TuyenDungCore.Models.Dtos.Employer;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.EF;
using Azure.Core;
using TuyenDungCore.Helpers;

namespace TuyenDungCore.DAO
{
    public class EmployerService
    {
        private readonly TuyenDungContext _context;
        public EmployerService()
        {
            _context = new TuyenDungContext();
        }

        public async Task<ResultPaging<EmployerVm>> GetList(GetListPaging paging)
        {
            var iQueryable = _context.NhaTuyenDungs.Where(m => !m.DeletedDate.HasValue);
            if (!string.IsNullOrEmpty(paging.KeyWord))
            {
                iQueryable = iQueryable.Where(x => x.Name.Contains(paging.KeyWord.Trim()));
            }

            int total = await iQueryable.CountAsync();

            var items = await iQueryable.OrderBy(x => x.Id)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .Select(item => new EmployerVm()
                {
                    Id = item.Id,
                    Address = item.Address,
                    Scale = item.Scale ?? "",
                    Phone = item.Phone,
                    Name = item.Name,
                    Website = item.Website ?? ""
                }).ToListAsync();
            return new ResultPaging<EmployerVm>()
            {
                Items = items,
                TotalRecord = total
            };
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.NhaTuyenDungs.FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return false;

            entity.DeletedDate = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> Create(EmployerCreateModel request)
        {
            var emailExists = await _context.Accounts.AsNoTracking().AnyAsync(m => m.Email == request.Email);
            if (emailExists) return -1;

            var accountE = new AccountEntity()
            {
                Email = request.Email,
                CreatedDate = DateTime.UtcNow,
                Password = Encryptor.MD5Hash(request.Password),
                Role = Enums.Roles.Employer,
            };
            accountE.NhaTuyenDung = new NhaTuyenDungEntity()
            {
                Email = request.Email,
                Address = request.Address,
                Name = request.CompanyName,
                Contact = request.Contact,
                Phone = request.Phone,
                Scale = request.Scale,
                Website = request.Website,
                Description = request.Description,
                Position = request.Position,
            };
            await _context.Accounts.AddAsync(accountE);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? 1 : -2;
        }

        public async Task<EmployerVm> GetById(int id)
        {
            var iQueryable = _context.NhaTuyenDungs.Where(m => !m.DeletedDate.HasValue && m.Id == id);
            var data = await iQueryable.Select(m => new EmployerVm
            {
                Address = m.Address,
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                Scale = m.Scale,
                Website = m.Website,
                Contact = m.Contact,
                Position = m.Position,
                Description = m.Description
            }).FirstOrDefaultAsync();
            return data;
        }
    }
}
