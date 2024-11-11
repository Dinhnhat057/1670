using Microsoft.AspNetCore.Mvc;
using TuyenDungCore.Commons;
using TuyenDungCore.DAO;
using TuyenDungCore.Enums;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.TinTuyenDung;

namespace TuyenDungCore.Areas.Admin.Controllers
{
    public class RecruitmentController : BaseController
    {
        private readonly TinTuyenDungService _tinTuyenDungService;
        public RecruitmentController()
        {
            _tinTuyenDungService = new TinTuyenDungService();
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPaging(TinTuyenDungStatus? status, string keyWord, int pageIndex = 1, int pageSize = 5, bool isDealine = false)
        {
            var userLogin = UserLogin();
            var request = new GetListPaging()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _tinTuyenDungService.GetList(isDealine, request, status, userLogin.Id);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _tinTuyenDungService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TinTuyenDungEdit request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _tinTuyenDungService.Update(request);
            if (result >= 0)
            {
                SetAlert("Cập nhật tin tuyển dụng thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                return View(request);
            }
        }

        public IActionResult TinChoDuyet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Approved(int id)
        {
            var result = await _tinTuyenDungService.Approved(id);
            if (result)
            {
                SetAlert("Duyệt thành công", "success");
            }
            else
            {
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }
            return Json(result);
        }
    }
}
