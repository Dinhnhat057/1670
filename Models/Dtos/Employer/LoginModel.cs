using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.Dtos.Employer
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; } = string.Empty;
    }
}
