using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.EF
{
    [Table("TinTuyenDungs")]
    public class TinTuyenDungEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Salary { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        [Required] public string Gender { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public string CandidateRequirements { get; set; } = string.Empty;
        public string? RelatedSkills { get; set; }
        public string? Right { get; set; }
        [Required] public DateTime Dealine { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int NhaTuyenDungId { get; set; }
        public TinTuyenDungStatus Status { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? Note { get; set; }

        public NhaTuyenDungEntity NhaTuyenDung { get; set; }
        public  ICollection<RecruitmentEntity> Recruitments { get; set; }
    }
}
