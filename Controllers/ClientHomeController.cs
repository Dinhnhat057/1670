using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text.Json;
using TuyenDungCore.Commons;
using TuyenDungCore.DAO;
using TuyenDungCore.Models;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.Candidate;
using TuyenDungCore.Models.Dtos.Employer;
using TuyenDungCore.Models.Dtos.TinTuyenDung;

namespace TuyenDungCore.Controllers
{
    public class ClientHomeController : Controller
    {
        private readonly TinTuyenDungService _tinTuyenDungService;
        private readonly AccountService _accountService;
        private readonly CandidateService _candidateService;

        private readonly ILogger<ClientHomeController> _logger;

        public ClientHomeController(ILogger<ClientHomeController> logger)
        {
            _logger = logger;
            _tinTuyenDungService = new TinTuyenDungService();
            _accountService = new AccountService();
            _candidateService = new CandidateService();
        }

        public async Task<IActionResult> Index()
        {
            var model = await _tinTuyenDungService.GetListItemHot(9);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search()
        {
            var model = await _tinTuyenDungService.GetListItemHot(5);
            return View(model);
        }

        public async Task<JsonResult> GetPaging(string KeyWord, int pageIndex)
        {
            var paging = new GetListPaging()
            {
                KeyWord = KeyWord,
                PageIndex = pageIndex,
                PageSize = 5
            };
            var data = await _tinTuyenDungService.GetListSearch(paging);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / paging.PageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        public async Task<IActionResult> Detail(int id)
        {
            UserLogin? userLogin = null;
            var userString = HttpContext.Session.GetString(CommonConstants.USER_SESSION);
            int? accountId = null;
            if (!string.IsNullOrEmpty(userString))
            {
                userLogin = JsonSerializer.Deserialize<UserLogin>(userString);
                accountId = userLogin.Id;
            }
            var model = await _tinTuyenDungService.GetRecruitmentById(id, accountId) ?? new TinTuyenDungDto();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {
            string data = "";
            if (ModelState.IsValid)
            {
                var result = await _accountService.Login(model.Email, model.Password, Enums.Roles.Seeker);
                if (result > 0)
                {
                    var userLogin = await _candidateService.GetByEmail(model.Email);
                    if (userLogin != null)
                    {
                        HttpContext.Session.SetString(Commons.CommonConstants.USER_SESSION, JsonSerializer.Serialize(userLogin));
                    }
                    TempData["Notify"] = "Đăng nhập thành công !";
                    TempData["AlertType"] = "alert-success";
                    return Json(new
                    {
                        success = true
                    });
                }
                else if (result == -3)
                {
                    data = "Tài khoản đã bị khóa";
                }
                else if (result == -1)
                {
                    data = "Sai tài khoản hoặc mật khẩu";
                }
            }
            return Json(new
            {
                data = data,
                success = false
            });
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] CandidateRegister model)
        {
            string data = "";
            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    var request = new CandidateCreateVm
                    {
                        Password = model.Password,
                        Email = model.Email,
                        Pasword_Confirm = model.ConfirmPassword,
                        Name = model.Name,
                    };
                    var result = await _candidateService.Create(request);
                    if (result > 0)
                    {
                        TempData["Notify"] = "Đăng ký thành công !";
                        TempData["AlertType"] = "alert-success";
                        return Json(new
                        {
                            success = true
                        });
                    }
                    else if (result == -1)
                    {
                        data = "Email này đã tồn tại";
                    }
                    else if (result == -2)
                    {
                        data = "Đã có lỗi xảy ra. Vui lòng thử lại";
                    }
                }
                else
                {
                    data = "Mật khẩu xác nhận chưa đúng";
                }
            }
            return Json(new
            {
                data = data,
                success = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> Recruitment(TinTuyenDungDto request)
        {
            UserLogin? userLogin = null;
            var userString = HttpContext.Session.GetString(CommonConstants.USER_SESSION);
            if (!string.IsNullOrEmpty(userString))
            {
                userLogin = JsonSerializer.Deserialize<UserLogin>(userString);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, request.FileCV.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.FileCV.CopyToAsync(stream);
                }

                var result = await _candidateService.Recruitment(request, userLogin.Id, request.FileCV.FileName);
                if (result > 0)
                {
                    TempData["Notify"] = "Ứng tuyển thành công !";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    TempData["Notify"] = "Đã có lỗi xảy ra";
                    TempData["AlertType"] = "alert-error";
                }
            }
            return RedirectToAction("Detail", "ClientHome", new { id = request.Id });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString(Commons.CommonConstants.USER_SESSION, "");
            TempData["Notify"] = "Đăng xuất thành công !";
            TempData["AlertType"] = "alert-success";
            return Redirect("/");
        }

        public async Task<IActionResult> Info()
        {
            UserLogin? userLogin = null;
            var userString = HttpContext.Session.GetString(CommonConstants.USER_SESSION);
            if (!string.IsNullOrEmpty(userString))
            {
                userLogin = JsonSerializer.Deserialize<UserLogin>(userString);
                var result = await _candidateService.GetById(userLogin.CandidateId.Value);
                var model = new CandidateEdit
                {
                    Address = result.Address,
                    Dob = string.IsNullOrEmpty(result.Dob) ? null : DateTime.Parse(result.Dob),
                    Gender = result.Gender,
                    Id = result.Id,
                    Name = result.Name,
                    Phone = result.Phone,
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Info(CandidateEdit request)
        {
            if(!ModelState.IsValid) return View(request);

            var result = await _candidateService.Update(request);
            if(result > 0)
            {
                TempData["Notify"] = "Cập nhật thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index");
            }    
            return View(request);
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
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.USER_SESSION);
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
                TempData["Notify"] = "Thay đổi mật khẩu thành công thành công";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Index", "ClientHome");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
