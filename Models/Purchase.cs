using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoShop.Models
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public string? Customer { get; set; }
        
        [ForeignKey("Motorcycle")]
        [StringLength(17, MinimumLength = 17)]
        public string? MotorcycleVIN { get; set; }
    }
}
