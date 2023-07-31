using System.ComponentModel.DataAnnotations;

namespace WinePage.Models
{
    public class Color
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public ICollection<WineColor> WineColors { get; set; }
    }
}
