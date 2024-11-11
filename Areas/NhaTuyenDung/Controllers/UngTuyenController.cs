using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using TuyenDungCore.DAO;
using TuyenDungCore.Models.Dtos;
using TuyenDungCore.Models.Dtos.UngTuyen;

namespace TuyenDungCore.Areas.NhaTuyenDung.Controllers
{
    public class UngTuyenController : BaseController
    {
        private readonly TinTuyenDungService _tinTuyenDungService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UngTuyenController(IWebHostEnvironment hostingEnvironment)
        {
            _tinTuyenDungService = new TinTuyenDungService();
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var userLogin = UserLogin();
            var data = await _tinTuyenDungService.GetRecruitmentByNhaTuyenDung(request, userLogin.NhaTuyenDungId.Value);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord });
        }

        public async Task<IActionResult> XemCV(int id)
        {
            var model = await _tinTuyenDungService.GetRecuritmentDetail(id) ?? new Models.Dtos.UngTuyen.UngTuyenVm();
            return View(model);
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var model = await _tinTuyenDungService.GetRecuritmentDetail(id) ?? new Models.Dtos.UngTuyen.UngTuyenVm();
            string webRootPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            string filePath = Path.Combine(webRootPath, model.FileCV);
            if (System.IO.File.Exists(filePath))
            {
                return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", model.FileCV);
                //return PhysicalFile(filePath, "application/octet-stream");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> CapNhat(int id)
        {
            var model = await _tinTuyenDungService.GetRecuritmentDetail(id);
            if (model == null) return RedirectToAction("Index", "UngTuyen");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhat(UngTuyenVm model)
        {
            var result = await _tinTuyenDungService.ChangeRecuritmentStatus(model);
            if (result == -1)
            {
                SetAlert("Không tìm thấy tin ứng tuyển", "error");
                return View(model);
            }
            else
            {
                SetAlert("Cập nhập trạng thái hồ sơ thành công", "success");
                return RedirectToAction("Index");
            }
        }
    }
}
