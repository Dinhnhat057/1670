namespace TuyenDungCore.Models.Dtos
{
    public class ResultPaging<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord { get; set; }
    }
}
