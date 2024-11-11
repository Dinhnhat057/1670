using Microsoft.EntityFrameworkCore;
using TuyenDungCore.Helpers;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Candidate;
using TuyenDungCore.Models.Dtos.TinTuyenDung;
using TuyenDungCore.Models.EF;

namespace TuyenDungCore.DAO
{
    public class CandidateService
    {
        private readonly TuyenDungContext _context;
        public CandidateService()
        {
            _context = new TuyenDungContext();
        }

        public async Task<ResultPaging<CandidateVm>> GetList(GetListPaging paging)
        {
            var iQueryable = _context.Candidates.Where(m => !m.DeletedDate.HasValue);
            if (!string.IsNullOrEmpty(paging.KeyWord))
            {
                iQueryable = iQueryable.Where(x => EF.Functions.Like(x.Phone, $"%{paging.KeyWord}%") || EF.Functions.Like(x.Name.ToLower(), $"%{paging.KeyWord.ToLower()}%"));
            }

            int total = await iQueryable.CountAsync();

            var items = await iQueryable.OrderBy(x => x.Id)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .Select(item => new CandidateVm()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Gender = item.Gender,
                    Dob = item.Dob.HasValue ? item.Dob.Value.ToString("dd-MM-yyyy") : "",
                    Address = item.Address,
                    Phone = item.Phone
                }).ToListAsync();
            return new ResultPaging<CandidateVm>()
            {
                Items = items,
                TotalRecord = total
            };
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Candidates.FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return false;

            entity.UpdatedDate = DateTime.Now;
            entity.DeletedDate = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> Create(CandidateCreateVm request)
        {
            var emailExists = await _context.Accounts.AsNoTracking().AnyAsync(m => m.Email == request.Email);
            if (emailExists) return -1;

            var accountE = new AccountEntity()
            {
                Email = request.Email,
                CreatedDate = DateTime.UtcNow,
                Password = Encryptor.MD5Hash(request.Password),
                Role = Enums.Roles.Seeker,
            };
            accountE.Candidate = new CandidateEntity()
            {
                Address = request.Address,
                Name = request.Name,
                Phone = request.Phone,
                Dob = request.Dob,
                Gender = request.Gender,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await _context.Accounts.AddAsync(accountE);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? 1 : -2;
        }

        public async Task<int> Update(CandidateEdit request)
        {
            var entity = await _context.Candidates.FirstOrDefaultAsync(m => m.Id == request.Id);
            if (entity == null) return -1;
            entity.Name = request.Name;
            entity.Address = request.Address;
            entity.Phone = request.Phone;
            entity.Dob = request.Dob;
            entity.Gender = request.Gender;
            return await _context.SaveChangesAsync();
        }

        public async Task<CandidateVm?> GetById(int id)
        {
            var iQueryable = _context.Candidates.Where(m => !m.DeletedDate.HasValue && m.Id == id);
            var data = await iQueryable.Select(m => new CandidateVm
            {
                Id = m.Id,
                Address = m.Address,
                Name = m.Name,
                Phone = m.Phone,
                Dob = m.Dob.HasValue ? m.Dob.Value.ToString("dd-MM-yyyy") : "",
                Gender = m.Gender,
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<UserLogin?> GetByEmail(string email)
        {
            var iQueryable = _context.Candidates.Join(_context.Accounts, c => c.AccountId, a => a.Id, (c, a) => new { c, a })
                .Where(m => m.a.Email == email);

            var result = await iQueryable.Select(m => new UserLogin
            {
                Email = m.a.Email,
                Id = m.a.Id,
                Name = m.c.Name,
                Role = Enums.Roles.Seeker,
                CandidateId = m.c.Id,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> Recruitment(TinTuyenDungDto request, int accountId, string fileCV)
        {
            var entity = new RecruitmentEntity
            {
                AccountId = accountId,
                CreatedDate = DateTime.Now,
                TinTuyenDungId = request.Id,
                Description = request.Introduction,
                UpdatedDate = DateTime.Now,
                FileCV = fileCV
            };
            await _context.Recruitments.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
