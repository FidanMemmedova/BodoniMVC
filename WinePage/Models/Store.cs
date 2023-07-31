using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WinePage.Attributes.ValidationAttributes;

namespace WinePage.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile ImageFile { get; set; }
    }
}
