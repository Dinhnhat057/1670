using Microsoft.EntityFrameworkCore;
using TuyenDungCore.Enums;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.TinTuyenDung;
using TuyenDungCore.Models.Dtos.UngTuyen;
using TuyenDungCore.Models.EF;

namespace TuyenDungCore.DAO
{
    public class TinTuyenDungService
    {
        private readonly TuyenDungContext _context;
        public TinTuyenDungService()
        {
            _context = new TuyenDungContext();
        }

        public async Task<int> Create(TinTuyenDungCreate request, int nhaTuyenDungId)
        {
            var entity = new TinTuyenDungEntity
            {
                Address = request.Address,
                CandidateRequirements = request.CandidateRequirements,
                CreatedDate = DateTime.Now,
                Dealine = request.Dealine,
                Description = request.Description,
                Gender = request.Gender,
                Name = request.Name,
                NhaTuyenDungId = nhaTuyenDungId,
                Quantity = request.Quantity,
                RelatedSkills = request.RelatedSkills,
                Right = request.Right,
                Salary = request.Salary,
                UpdatedDate = DateTime.Now,
                Status = TinTuyenDungStatus.InProgress
            };
            await _context.TinTuyenDungs.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultPaging<TinTuyenDungDto>> GetList(bool isDealine, GetListPaging paging, TinTuyenDungStatus? status, int accountId, Roles role = Roles.QUANTRIVIEN)
        {
            var iQueryable = _context.TinTuyenDungs.Join(_context.NhaTuyenDungs, t => t.NhaTuyenDungId, n => n.Id, (t, n) => new { t, n })
                .Where(m => !m.t.DeletedDate.HasValue);
            
            if (role == Roles.Employer)
            {
                var employer = await _context.NhaTuyenDungs.FirstOrDefaultAsync(m => m.AccountId == accountId);
                iQueryable = iQueryable.Where(m => m.t.NhaTuyenDungId == employer.Id);
            }

            if(isDealine) 
                iQueryable = iQueryable.Where(m => m.t.Dealine < DateTime.Now);
            
            if (status.HasValue)
                iQueryable = iQueryable.Where(m => m.t.Status == status.Value);
            
            if (!string.IsNullOrEmpty(paging.KeyWord))
                iQueryable = iQueryable.Where(m => EF.Functions.Like(m.t.Name.ToLower(), $"%{paging.KeyWord.ToLower()}%"));

            var totalRecord = await iQueryable.CountAsync();

            iQueryable = iQueryable.OrderByDescending(m => m.t.CreatedDate)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var results = await iQueryable.Select(m => new TinTuyenDungDto
            {
                CreatedDate = m.t.CreatedDate.ToString("dd-MM-yyyy"),
                Dealine = m.t.Dealine.ToString("dd-MM-yyyy"),
                Id = m.t.Id,
                Name = m.t.Name,
                Quatity = m.t.Quantity,
                Status = m.t.Status == TinTuyenDungStatus.InProgress ? "Đang chờ duyệt" : (m.t.Status == TinTuyenDungStatus.Approved ? "Đã duyệt" : "Từ chối"),
                TenNTD = m.n.Name
            }).ToListAsync();

            return new ResultPaging<TinTuyenDungDto>
            {
                TotalRecord = totalRecord,
                Items = results
            };
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _context.TinTuyenDungs.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null) return 0;
            entity.DeletedDate  = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            return await _context.SaveChangesAsync();
        }

        public async Task<TinTuyenDungEdit?> GetById(int id)
        {
            var entity = await _context.TinTuyenDungs.FirstOrDefaultAsync(m => m.Id == id && !m.DeletedDate.HasValue);
            if (entity == null) return null;

            var result = new TinTuyenDungEdit
            {
                Address = entity.Address,
                CandidateRequirements = entity.CandidateRequirements,
                Dealine = entity.Dealine,
                Description = entity.Description,
                Gender = entity.Gender,
                Id = entity.Id,
                Name = entity.Name,
                Quantity = entity.Quantity,
                RelatedSkills = entity.RelatedSkills,
                Right = entity.Right,
                Salary = entity.Salary,
                Status = entity.Status,
                Note = entity.Note
            };
            return result;
        }

        public async Task<int> Update(TinTuyenDungEdit request)
        {
            var entity = await _context.TinTuyenDungs.FirstOrDefaultAsync(m => m.Id == request.Id);
            if (entity == null ) return -1;

            entity.UpdatedDate = DateTime.Now;
            entity.CandidateRequirements = request.CandidateRequirements;
            entity.Dealine = request.Dealine;
            entity.Description = request.Description;
            entity.Gender = request.Gender;
            entity.Address = request.Address;
            entity.Right = request.Right;
            entity.Salary = request.Salary;
            entity.Name = request.Name;
            entity.Quantity = request.Quantity;
            entity.RelatedSkills = request.RelatedSkills;
            entity.Note = request.Note;
            entity.Status = request.Status.HasValue ? request.Status.Value : entity.Status;

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> Approved(int id)
        {
            try
            {
                var entity = await _context.TinTuyenDungs.FirstOrDefaultAsync(m => m.Id == id);
                if (entity == null) return false;
                entity.Status = TinTuyenDungStatus.Approved;
                entity.UpdatedDate = DateTime.Now;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<TinTuyenDungDto>> GetListItemHot(int limit = 9) 
        {
            var iQueryable = _context.TinTuyenDungs.Join(_context.NhaTuyenDungs, t => t.NhaTuyenDungId, n => n.Id, (t, n) => new { t, n })
                .Where(m => m.t.Status == TinTuyenDungStatus.Approved && !m.t.DeletedDate.HasValue);

            iQueryable = iQueryable.OrderByDescending(m => m.t.CreatedDate).Take(9);

            var results = await iQueryable.Select(m => new TinTuyenDungDto
            {
                Address = m.t.Address,
                CreatedDate = m.t.CreatedDate.ToString("dd-MM-yyyy"),
                Dealine = m.t.Dealine.ToString("dd-MM-yyyy"),
                Id = m.t.Id,
                Name = m.t.Name,
                Quatity = m.t.Quantity,
                Salary = m.t.Salary,
                TenNTD = m.n.Name,
            }).ToListAsync();

            return results;
        }

        public async Task<ResultPaging<TinTuyenDungDto>> GetListSearch(GetListPaging paging)
        {
            var iQueryable = _context.TinTuyenDungs.Join(_context.NhaTuyenDungs, t => t.NhaTuyenDungId, n => n.Id, (t, n) => new { t, n })
                 .Where(m => !m.t.DeletedDate.HasValue && m.t.Status == TinTuyenDungStatus.Approved);
            
            if (!string.IsNullOrEmpty(paging.KeyWord))
                iQueryable = iQueryable.Where(m => EF.Functions.Like(m.t.Name.ToLower(), $"%{paging.KeyWord.ToLower()}%") || EF.Functions.Like(m.t.Address.ToLower(), $"%{paging.KeyWord.ToLower()}%"));

            var totalRecord = await iQueryable.CountAsync();

            iQueryable = iQueryable.OrderByDescending(m => m.t.CreatedDate)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var results = await iQueryable.Select(m => new TinTuyenDungDto
            {
                CreatedDate = m.t.CreatedDate.ToString("dd-MM-yyyy"),
                Dealine = m.t.Dealine.ToString("dd-MM-yyyy"),
                Id = m.t.Id,
                Name = m.t.Name,
                Quatity = m.t.Quantity,
                Status = m.t.Status == TinTuyenDungStatus.InProgress ? "Đang chờ duyệt" : (m.t.Status == TinTuyenDungStatus.Approved ? "Đã duyệt" : "Từ chối"),
                TenNTD = m.n.Name,
                Salary = m.t.Salary,
                Address = m.t.Address
            }).ToListAsync();

            return new ResultPaging<TinTuyenDungDto>
            {
                TotalRecord = totalRecord,
                Items = results
            };
        }

        public async Task<TinTuyenDungDto?> GetRecruitmentById(int id, int? accountId = null)
        {
            var iQueryable = _context.TinTuyenDungs.Join(_context.NhaTuyenDungs, t => t.NhaTuyenDungId, n => n.Id, (t, n) => new { t, n })
                .GroupJoin(_context.Recruitments, t1 => t1.t.Id, r => r.TinTuyenDungId, (t1, r) => new { t = t1.t, n = t1.n, r  });

            iQueryable = iQueryable.Where(m => m.t.Id == id && !m.t.DeletedDate.HasValue);

            var result = await iQueryable.Select(m => new TinTuyenDungDto
            {
                Address = m.t.Address,
                CandidateRequirements = m.t.CandidateRequirements,
                CreatedDate = m.t.CreatedDate.ToString("dd-MM-yyyy"),
                Dealine = m.t.Dealine.ToString("dd-MM-yyyy"),
                Description = m.t.Description,
                DescriptionNTD = m.n.Description,
                Gender = m.t.Gender,
                Id = m.t.Id,
                Name = m.t.Name,
                Quatity = m.t.Quantity,
                RelatedSkills = m.t.RelatedSkills,
                Right = m.t.Right,
                Salary = m.t.Salary,
                Scale = m.n.Scale,
                TenNTD = m.n.Name,
                IsApply = m.r != null && m.r.Any(z => z.AccountId == accountId)
            }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResultPaging<UngTuyenVm>> GetRecruitmentByNhaTuyenDung(GetListPaging paging, int nhaTuyenDungId)
        {
            var iQueryable = _context.Recruitments.Include(m => m.TinTuyenDung).Include(m => m.Account)
                .Where(m => m.TinTuyenDung.NhaTuyenDungId == nhaTuyenDungId);

            if(!string.IsNullOrWhiteSpace(paging.KeyWord))
            {
                iQueryable = iQueryable.Where(m => EF.Functions.Like(m.TinTuyenDung.Name.ToLower(), $"%{paging.KeyWord.ToLower()}%"));
            }

            var totalRecord = await iQueryable.CountAsync();
            iQueryable = iQueryable.OrderByDescending(m => m.CreatedDate)
                 .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var results = await iQueryable.Select(m => new UngTuyenVm
            {
                CandidateId = m.Account.Candidate.Id,
                CandidateName = m.Account.Candidate.Name,
                JobName = m.TinTuyenDung.Name,
                RecruitmentDate = m.CreatedDate.ToString("dd-MM-yyyy"),
                TinTuyenDungId = m.TinTuyenDungId,
                Id = m.Id
            }).ToListAsync();

            return new ResultPaging<UngTuyenVm> { Items = results, TotalRecord = totalRecord };
        }

        public async Task<UngTuyenVm?> GetRecuritmentDetail(int id)
        {
            var iQueryable = _context.Recruitments.Include(m => m.TinTuyenDung).Include(m => m.Account)
                .Where(m => m.Id == id);

            var result = await iQueryable.Select(m => new UngTuyenVm
            {
                CandidateId = m.Account.Candidate.Id,
                CandidateName = m.Account.Candidate.Name,
                JobName = m.TinTuyenDung.Name,
                RecruitmentDate = m.CreatedDate.ToString("dd-MM-yyyy"),
                TinTuyenDungId = m.TinTuyenDungId,
                CandidatePhone = m.Account.Candidate.Phone ?? "",
                Gender = m.Account.Candidate.Gender ?? "",
                Introduction = m.Description,
                Id = m.Id,
                Status = m.Status,
                FileCV = m.FileCV,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> ChangeRecuritmentStatus(UngTuyenVm model)
        {
            var entity = await _context.Recruitments.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (entity == null) return -1;
            entity.UpdatedDate = DateTime.Now;
            entity.Status = model.Status;

            return await _context.SaveChangesAsync();
        }
    }
}
