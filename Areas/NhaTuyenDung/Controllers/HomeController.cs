using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Employer;

namespace TuyenDungCore.Areas.NhaTuyenDung.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AccountService accountService;
        public HomeController()
        {
            accountService = new AccountService();
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Info()
        {
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.EMPLOYER_SESSION);
            if (!string.IsNullOrEmpty(accountString))
            {
                var account = JsonSerializer.Deserialize<UserLogin>(accountString);
                var result = await accountService.GetNhaTuyenDungInfo(account.Id);
                return View(result);
            }    
            return View();
        }

        [HttpPost]
        public IActionResult Info(EmployerEditClient request) 
        { 
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString(Commons.CommonConstants.EMPLOYER_SESSION, "");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserPassword request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.EMPLOYER_SESSION);
            if (!string.IsNullOrEmpty(accountString))
            {
                var account = JsonSerializer.Deserialize<UserLogin>(accountString);
                var result = await accountService.ChangePassword(request, account.Id);
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
                return RedirectToAction("Index", "TinTuyenDung");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
