using Microsoft.EntityFrameworkCore;
using TuyenDungCore.Enums;
using TuyenDungCore.Helpers;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Employer;
using TuyenDungCore.Models.EF;

namespace TuyenDungCore.DAO
{
    public class AccountService
    {
        private readonly TuyenDungContext _context;
        public AccountService()
        {
            _context = new TuyenDungContext();
        }

        public async Task<int> Create(RegisterModel request)
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
                
            };
            await _context.Accounts.AddAsync(accountE);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? 1 : -2;
        }

        public async Task<int> Login(string email, string password, Roles roles)
        {
            var accountE = await _context.Accounts.FirstOrDefaultAsync(m => m.Email == email && m.Role == roles);
            if (accountE == null) return -1;// Thông tin Email không tồn tại

            if (accountE.Status == Status.Active)
            {
                if (accountE.Password != Encryptor.MD5Hash(password)) return -2;// Thông tin mật khẩu không đúng

                if(roles == Roles.Employer)
                {
                    var hasDeleted = await _context.NhaTuyenDungs.AsNoTracking().AnyAsync(m => m.AccountId == accountE.Id && m.DeletedDate.HasValue);
                    if (hasDeleted) return -4;
                }
            }
            else return -3; // Tài khoản đã bị khóa

            return 1;
        }

        public async Task<UserLogin?> GetNhaTuyenDungByEmail(string email)
        {
            var iQueryable = _context.Accounts.Join(_context.NhaTuyenDungs, a => a.Id, n => n.AccountId, (a, n) => new { a, n })
                .Where(m => m.a.Email == email && m.a.Role == Roles.Employer);

            var data = await iQueryable.Select(m => new UserLogin
            {
                Role = m.a.Role,
                Email = m.a.Email,
                Id = m.a.Id,
                Name = m.n.Name,
                NhaTuyenDungId = m.n.Id,
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<UserLogin?> GetAdminByEmail(string email)
        {
            var iQueryable = _context.Accounts.Where(m => m.Email == email);
            var data = await iQueryable.Select(m => new UserLogin
            {
                Role = m.Role,
                Email = m.Email,
                Id = m.Id,
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<EmployerEditClient> GetNhaTuyenDungInfo(int accountId)
        {
            var iQueryable = _context.NhaTuyenDungs.Where(m => m.AccountId == accountId);
            var data = await iQueryable.Select(m => new EmployerEditClient
            {
                Address = m.Address,
                Contact = m.Contact,
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
            }).FirstOrDefaultAsync();
            return data ?? new EmployerEditClient();
        }

        public async Task<ResultPaging<AccountVm>> GetList(GetListPaging paging)
        {
            var iQueryable = _context.Accounts.Where(m => !m.DeletedDate.HasValue && m.Role != Roles.QUANTRIVIEN);

            if (!string.IsNullOrEmpty(paging.KeyWord))
            {
                iQueryable = iQueryable.Where(m => EF.Functions.Like(m.Email.ToLower(), $"%{paging.KeyWord.ToLower()}%"));
            }

            int total = await iQueryable.CountAsync();

            var items = await iQueryable.OrderBy(x => x.Id)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .Select(m => new AccountVm
                {
                    CreatedDate = m.CreatedDate.ToString("dd-MM-yyyy"),
                    Email = m.Email,
                    Role = m.Role == Roles.QUANTRIVIEN ? "Quản trị viên" : (m.Role == Roles.Seeker ? "Ứng viên" : "Nhà tuyển dụng"),
                    Id = m.Id,
                    Status = m.Status == Status.InActive ? "Khóa" : "Kích hoạt"
                })
                .ToListAsync();
            return new ResultPaging<AccountVm>()
            {
                Items = items,
                TotalRecord = total
            };
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return false;
            entity.DeletedDate = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LockAccount(int id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return false;
            entity.Status = Status.InActive;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnLockAccount(int id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return false;
            entity.Status = Status.Active;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> ChangePassword(UserPassword request, int accountId)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == accountId);
            if (entity == null) return -2;

            if (entity.Password != Encryptor.MD5Hash(request.OldPassword)) return -1; // Mật khẩu cũ không chính xác

            entity.Password = Encryptor.MD5Hash(request.Password);

            return await _context.SaveChangesAsync();

        }
    }
}
