using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TuyenDungCore.Areas.Admin.Models;
using TuyenDungCore.Commons;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;

namespace TuyenDungCore.Areas.Admin.Controllers
{  
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController()
        {
            _accountService = new AccountService();
        }
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _accountService.Login(request.UserName, request.PassWord, Enums.Roles.QUANTRIVIEN);
            if (result == -1)
            {
                ModelState.AddModelError("Email", "Thông tin tên đăng nhập không tồn tại.");
                return View(request);
            }
            else if (result == -2)
            {
                ModelState.AddModelError("Password", "Thông tin mật khẩu không chính xác.");
                return View(request);
            }
            else if (result == -3)
            {
                ModelState.AddModelError("Email", "Tài khoản bị khóa.");
                return View(request);
            }
            var userLogin = await _accountService.GetAdminByEmail(request.UserName);
            if (userLogin != null)
            {
                HttpContext.Session.SetString(Commons.CommonConstants.ADMIN_SESSION, JsonSerializer.Serialize(userLogin));
                TempData["AlertMessage"] = "Đăng nhập thành công !";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index", "Recruitment");
            }
            return View(request);
        }

        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _accountService.GetList(request);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.Delete(id);
            if (result)
            {
                TempData["Notify"] = "Xóa thành công";
                TempData["AlertType"] = "alert-success";
            }
            else
            {
                TempData["Notify"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                TempData["AlertType"] = "alert-danger";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> LockAccount(int id)
        {
            var result = await _accountService.LockAccount(id);
            if (result)
            {
                TempData["Notify"] = "Khóa tài khoản thành công";
                TempData["AlertType"] = "alert-success";
            }
            else
            {
                TempData["Notify"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                TempData["AlertType"] = "alert-danger";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UnLockAccount(int id)
        {
            var result = await _accountService.UnLockAccount(id);
            if (result)
            {
                TempData["Notify"] = "Mở khóa tài khoản thành công";
                TempData["AlertType"] = "alert-success";
            }
            else
            {
                TempData["Notify"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                TempData["AlertType"] = "alert-danger";
            }
            return Json(result);
        }
    }
}
