using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungCore.Models.EF
{
    [Table("NhaTuyenDungs")]
    public class NhaTuyenDungEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public string? Scale { get; set; }
        public string? Website { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? Position { get; set; }
        public string? Description { get; set; }
        public AccountEntity Account { get; set; }
        public ICollection<TinTuyenDungEntity> TinTuyenDungs { get; set; } = new List<TinTuyenDungEntity>();
    }
}
