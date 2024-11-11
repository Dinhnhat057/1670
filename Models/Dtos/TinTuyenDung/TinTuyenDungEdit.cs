using System.ComponentModel.DataAnnotations;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.Dtos.TinTuyenDung
{
    public class TinTuyenDungEdit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên công việc")]
        [MaxLength(length: 250, ErrorMessage = "Nên đặt tên công việc dưới 250 ký tự")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Chưa chọn mức lương")]
        public string Salary { get; set; } = string.Empty;
        [Required(ErrorMessage = "Chưa nhập địa chỉ làm việc")]
        public string Address { get; set; } = string.Empty;

        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Chưa chọn yêu cầu giới tính")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chưa nhập mô tả công việc")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chưa nhập yêu cầu ứng viên")]
        public string CandidateRequirements { get; set; } = string.Empty;

        public string? RelatedSkills { get; set; }

        public string? Right { get; set; }

        [Required(ErrorMessage = "Chưa nhập hạn nộp")]
        public DateTime Dealine { get; set; }

        public TinTuyenDungStatus? Status { get; set; }
        public string? Note { get; set; }
    }
}
