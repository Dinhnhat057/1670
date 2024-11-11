using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.Dtos
{
    public class UserPassword
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận chưa đúng")]
        public string ConfirmPassword { get; set; }
    }
}
