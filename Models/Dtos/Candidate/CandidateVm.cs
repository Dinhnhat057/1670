using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.Dtos.Candidate
{
    public class CandidateVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên ứng viên")]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public string Dob { get; set; }

        public string Address { get; set; }
    }

    public class CandidateCreateVm
    {
        [Required(ErrorMessage = "Bạn phải tên ứng viên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn giới tính")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn ngày sinh")]
        public DateTime Dob { get; set; }

        public string Address { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận chưa đúng")]
        public string Pasword_Confirm { get; set; } = string.Empty;
        [Display(Name = "Email đăng nhập")]
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email chưa đúng định dạng")]
        public string Email { get; set; } = string.Empty;
    }

    public class CandidateRegister
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CandidateEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên ứng viên")]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public DateTime? Dob { get; set; }

        public string Address { get; set; }
    }
}
