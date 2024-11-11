using Microsoft.VisualStudio.TextTemplating;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.Dtos.TinTuyenDung
{
    public class TinTuyenDungDto
    {
        public int Id { get; set; }

        public string TenNTD { get; set; }

        public string Name { get; set; }

        public int? Quatity { get; set; }

        public string CreatedDate { get; set; }

        public string Dealine { get; set; }

        public string Status { get; set; }
        public string Address { get; set; }
        public string Salary { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string CandidateRequirements { get; set; }
        public string? RelatedSkills { get; set; }
        public string? Right { get; set; }
        public string? Scale { get; set; }
        public string? DescriptionNTD { get; set; }
        public string Introduction { get; set; }
        public bool IsApply { get; set; }
        public IFormFile FileCV { get; set; }
    }
}
