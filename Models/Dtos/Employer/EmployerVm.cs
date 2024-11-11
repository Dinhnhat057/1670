namespace TuyenDungCore.Models.Dtos.Employer
{
    public class EmployerVm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string? Scale { get; set; }

        public string Address { get; set; }

        public string? Website { get; set; }
        public string? Contact { get; set; }
        public string? Position { get; set; }
        public string? Description { get; set; }
    }
}
