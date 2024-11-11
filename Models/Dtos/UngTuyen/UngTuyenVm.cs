using Microsoft.VisualStudio.TextTemplating;
using TuyenDungCore.Enums;

namespace TuyenDungCore.Models.Dtos.UngTuyen
{
    public class UngTuyenVm
    {
        public int CandidateId { get; set; }

        public int TinTuyenDungId { get; set; }
        public string CandidateName { get; set; }
        public string CandidatePhone { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }

        public string JobName { get; set; }

        public string RecruitmentDate { get; set; }
        public string FileCV { get; set; }
        public int Id { get; set; }
        public RecruitmentStatus Status { get; set; }
    }
}
