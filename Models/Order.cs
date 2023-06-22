using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public string MotorcycleVIN { get; set; }
    }
}
