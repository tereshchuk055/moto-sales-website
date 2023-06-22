using MotoShop.Data.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand Name field is required!")]
        [DataType(DataType.Text)]
        public string? BrandName { get; set; }
    }
}
