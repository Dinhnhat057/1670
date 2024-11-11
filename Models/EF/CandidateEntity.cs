using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.EF
{
    [Table("Candidates")]
    public class CandidateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public DateTime? Dob { get; set; }

        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
        public DateTime? DeletedDate { get; set; }
        public int AccountId { get; set; }

        public AccountEntity Account { get; set; }
    }
}
