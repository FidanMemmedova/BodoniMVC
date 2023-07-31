using System.ComponentModel.DataAnnotations;

namespace WinePage.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public ICollection<Wine> Wine { get; set; }
    }
}
