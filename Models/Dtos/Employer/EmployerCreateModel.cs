using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.Dtos.Employer
{
    public class EmployerCreateModel
    {
        [Display(Name = "Tên nhà tuyển dụng")]
        [Required(ErrorMessage = "Bạn chưa nhập tên nhà tuyển dụng")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Email đăng nhập")]
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email chưa đúng định dạng")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Người liên hệ")]
        [Required(ErrorMessage = "Bạn chưa tên người liên hệ")]
        public string Contact { get; set; } = string.Empty;

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận chưa đúng")]
        public string Pasword_Confirm { get; set; } = string.Empty;

        public string? Position { get; set; }
        public string? Scale { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
    }
}
