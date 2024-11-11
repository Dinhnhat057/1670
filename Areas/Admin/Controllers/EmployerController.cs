using Microsoft.AspNetCore.Mvc;
using System;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Employer;

namespace TuyenDungCore.Areas.Admin.Controllers
{
    public class EmployerController : BaseController
    {
        private readonly EmployerService _employerService;
        public EmployerController()
        {
            _employerService = new EmployerService();
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
            var data = await _employerService.GetList(request);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employerService.Delete(id);
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
        public async Task<IActionResult> Create(EmployerCreateModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _employerService.Create(request);
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
            var model = await _employerService.GetById(id);
            return View(model);
        }
    }
}
