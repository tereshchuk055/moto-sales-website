using MotoShop.Data.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    [Table("Type")]
    public class Type
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? TypeName { get; set; }
    }
}
