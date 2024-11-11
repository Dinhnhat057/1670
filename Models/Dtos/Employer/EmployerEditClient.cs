using Microsoft.VisualStudio.TextTemplating;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.Dtos.Employer
{
    public class EmployerEditClient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Chưa nhập tên nhà tuyển dụng")]
        public string Name { get; set; } = string.Empty;

        //public string TieuDe => StringHelper.ToUnsignString(TenNTD).ToLower();

        [Required(ErrorMessage = "Chưa nhập tên người đại diện")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chưa nhập chức vụ người đại diện")]
        public string Position { get; set; } = string.Empty;

        public string Avartar { get; set; } = string.Empty;

        public string AnhBia { get; set; }

        [Required(ErrorMessage = "Chưa nhập số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        [MaxLength(length: 250, ErrorMessage = "Hãy mô tả dưới 250 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chưa nhập địa chỉ công ty")]
        public string Address { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public IFormFile? ImageMain { get; set; }
        public IFormFile? ImageCover { get; set; }
    }
}
