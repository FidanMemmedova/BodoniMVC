using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WinePage.Attributes.ValidationAttributes;

namespace WinePage.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public string Desc { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile ImageFile { get; set; }
       

    }
}
