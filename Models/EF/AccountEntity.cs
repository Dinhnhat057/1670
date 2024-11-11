using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.EF
{
    [Table("Accounts")]
    public class AccountEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public Roles Role { get; set; }
        public Status Status { get; set; }
        public DateTime? DeletedDate { get; set; }

        public NhaTuyenDungEntity NhaTuyenDung { get; set; }
        public CandidateEntity Candidate { get; set; }

        public ICollection<RecruitmentEntity> Recruitments { get; set; }
    }
}
