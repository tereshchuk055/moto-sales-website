using MotoShop.Data.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MotoShop.ViewModels.MotorcycleViewModels
{
    public class MotorcycleCreateViewModel
    {
        [Key]
        [Required]
        [UniqueVIN]
        [StringLength(17, MinimumLength = 17)]
        public string? VIN { get; init; }

        [Required]
        [ForeignKey("Model")]
        public int ModelId { get; init; }

        [Required]
        [ForeignKey("Type")]
        public int TypeId { get; set; }

        [Required]
        [IsPositive]
        public string EngineDisplacement { get; set; }

        [Required]
        [IsPositive]
        public float Price { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Manufactured { get; init; }

        [Required]
        public IFormFile Photo { get; init; }

        [ScaffoldColumn(false)]
        public bool Visible { get; set; } = true;
    }
}
