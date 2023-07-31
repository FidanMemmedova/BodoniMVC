namespace WinePage.Models
{
    public class WineColor
    {
        public int Id { get; set; }
        public int WineId { get; set; }
        public int ColorId { get; set; }

        public Wine wine { get; set; }
        public Color color { get; set; }
    }
}
