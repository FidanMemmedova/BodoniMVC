using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WinePage.Attributes.ValidationAttributes;

namespace WinePage.Models
{
    public class OurClubCard
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile IconFile { get; set; }
    }
}
