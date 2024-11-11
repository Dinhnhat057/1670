using Microsoft.AspNetCore.Mvc;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Candidate;

namespace TuyenDungCore.Areas.Admin.Controllers
{
    public class CandidateController : BaseController
    {
        private readonly CandidateService _candidateService;
        public CandidateController()
        {
            _candidateService = new CandidateService();
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _candidateService.GetList(request);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _candidateService.Delete(id);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }
            return Json(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CandidateCreateVm request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _candidateService.Create(request);
            if (result == -1)
            {
                ModelState.AddModelError("Email", "Thông tin email đã tồn tại");
                return View(request);
            }
            else if (result == -2)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại");
                return View(request);
            }
            TempData["notify"] = "Đăng ký thành công !";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = await _candidateService.GetById(id);
            return View(model);
        }
    }
}
