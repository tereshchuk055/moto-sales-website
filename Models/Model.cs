using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    [Table("Model")]
    public class Model
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Brand")]
        public int? BrandId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? ModelName { get; set; }
    }
}
