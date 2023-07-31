using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WinePage.Attributes.ValidationAttributes;

namespace WinePage.Models
{
    public class Wine
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; } 
        public int CategoryId { get; set; }
        public int PropertyId { get; set; }
        public int BrandId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Property Property { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [MaxLength(100)]
        public string Image { get; set; }

        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile ImageFile { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [Required]
        public List<int> ColorIds { get; set; } 
        public List<WineColor> WineColors { get; set; }

    }
}
