using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;

namespace TuyenDungCore.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly AccountService _accountService;
        public DashboardController()
        {
            _accountService = new AccountService();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserPassword request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.ADMIN_SESSION);
            if (!string.IsNullOrEmpty(accountString))
            {
                var account = JsonSerializer.Deserialize<UserLogin>(accountString);
                var result = await _accountService.ChangePassword(request, account.Id);
                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Mật khẩu mới trùng với mật khẩu cũ");
                    return View(request);
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác");
                    return View(request);
                }
                SetAlert("Thay đổi mật khẩu thành công thành công", "success");
                return RedirectToAction("Index", "Account");
            }

            return RedirectToAction("Index", "Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString(Commons.CommonConstants.ADMIN_SESSION, "");
            return RedirectToAction("Login", "Account");
        }
    }
}
