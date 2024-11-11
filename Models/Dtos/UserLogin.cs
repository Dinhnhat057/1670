using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.Dtos
{
    public class UserLogin
    {
        public int? NhaTuyenDungId { get; set; }
        public int? CandidateId { get; set; }
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Roles Role { get; set; }
    }
}
