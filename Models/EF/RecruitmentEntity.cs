using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.EF
{
    [Table("Recruitments")]
    public class RecruitmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int AccountId { get; set; }
        
        public int TinTuyenDungId { get; set; }
        public string Description { get; set; }
        public string FileCV { get; set; }

        [ForeignKey("TinTuyenDungId")]
        public  TinTuyenDungEntity TinTuyenDung { get; set; }
        public  AccountEntity Account { get; set; }
        public RecruitmentStatus Status { get; set; }
    }
}
