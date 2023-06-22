using System.ComponentModel.DataAnnotations;

namespace MotoShop.DataTransferObjects
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string? Customer { get; set; }
        public string? VIN { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public double EngineDisplacement { get; set; }
        public DateOnly Manufactured { get; set; }
        public double Price { get; set; }

    }
}
