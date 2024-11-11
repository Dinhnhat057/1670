using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TuyenDungCore.Commons;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Employer;

namespace TuyenDungCore.Areas.NhaTuyenDung.Controllers
{
    public class LoginController : Controller
    {
        private readonly AccountService accountService;
        public LoginController()
        {
            accountService = new AccountService();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await accountService.Login(request.Email, request.Password, Enums.Roles.Employer);
            if (result == -1)
            {
                ModelState.AddModelError("Email", "Thông tin E-mail không tồn tại.");
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
            else if (result == -4)
            {
                ModelState.AddModelError("Email", "Tài khoản bị xóa.");
                return View(request);
            }
            var userLogin = await accountService.GetNhaTuyenDungByEmail(request.Email);
            if (userLogin != null)
            {
                HttpContext.Session.SetString(Commons.CommonConstants.EMPLOYER_SESSION, JsonSerializer.Serialize(userLogin));
                TempData["Notify"] = "Đăng nhập thành công!";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index", "TinTuyenDung");
            }
            return View(request);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await accountService.Create(request);
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
    }
}
