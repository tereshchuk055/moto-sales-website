using MotoShop.Data.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MotoShop.ViewModels.MotorcycleViewModels;

namespace MotoShop.Models
{
    [Table("Motorcycle")]
    public class Motorcycle
    {
        [Key]
        [Required]
        [UniqueVIN]
        [StringLength(17, MinimumLength = 17)]
        public string? VIN { get; init; }

        [Required]
        [ForeignKey("Model")]
        public int? ModelId { get; init; }

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
        public DateOnly Manufactured { get; init; }

        [ScaffoldColumn(false)]
        public string? PhotoPath { get; set; }

        [ScaffoldColumn(false)]
        public bool Visible { get; set; } = true;

        public Motorcycle() {}

        public Motorcycle(MotorcycleEditViewModel model, string path)
        {
            VIN = model.VIN;
            ModelId = model.ModelId;
            TypeId = model.TypeId;
            EngineDisplacement = model.EngineDisplacement;
            Price = model.Price;
            Manufactured = model.Manufactured;
            PhotoPath = path;
            Visible = model.Visible;
        }

    }
}
