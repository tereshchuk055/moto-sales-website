using MotoShop.Data.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.ViewModels.MotorcycleViewModels
{
    public class MotorcycleEditViewModel
    {
        [ScaffoldColumn(false)]
        public string? VIN { get; set; }

        [Required]
        [ForeignKey("Model")]
        public int? ModelId { get; init; }

        [ScaffoldColumn(false)]
        public int? BrandId { get; init; }

        [Required]
        [ForeignKey("Type")]
        public int? TypeId { get; set; }

        [Required]
        [IsPositive]
        public string EngineDisplacement { get; set; }

        [Required]
        [IsPositive]
        public float Price { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Manufactured { get; init; }

        public IFormFile? Photo { get; init; }

        [Required]
        public bool Visible { get; init; }
    }
}
