namespace MotoShop.DataTransferObjects
{
    public class MotorcycleDto
    {
        public string? VIN { get; init; }
        public string? Model { get; init; }
        public string? Brand { get; init; }
        public string? Type { get; init; }
        public string? Manufactured { get; init; }
        public string? Photo { get; set; }
        public double? Price { get; set; }
        public double? EngineDisplacement { get; set; }
        public bool? Visible { get; set; }
    }
}
