using Microsoft.AspNetCore.Mvc;
using TuyenDungCore.Commons;
using TuyenDungCore.DAO;
using TuyenDungCore.Enums;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.TinTuyenDung;

namespace TuyenDungCore.Areas.NhaTuyenDung.Controllers
{
    public class TinTuyenDungController : BaseController
    {
        private readonly TinTuyenDungService _tinTuyenDungService;
        public TinTuyenDungController()
        {
            _tinTuyenDungService = new TinTuyenDungService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TinTuyenDungCreate request)
        {
            if (!ModelState.IsValid) 
            { 
                return View(request);
            }
            var userLogin = UserLogin();
            if (userLogin == null) return RedirectToAction("Index", "Login");
            var result = await _tinTuyenDungService.Create(request, userLogin.NhaTuyenDungId ?? 0);
            if (result > 0)
            {
                SetAlert("Tạo tin tuyển dụng thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                return View(request);
            }
            
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
            var data = await _tinTuyenDungService.GetList(isDealine, request, status, userLogin.Id, Roles.Employer);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tinTuyenDungService.Delete(id);
            if (result > 0)
            {
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }
            return Json(result);
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

        public IActionResult ChoDuyet()
        {
            return View();
        }

        public IActionResult HetHan()
        {
            return View();
        }
    }
}
