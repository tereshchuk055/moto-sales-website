using MotoShop.Models;

namespace MotoShop.DataTransferObjects
{
    public class PricesByModelsDto
    {
        public string BrandName { get; set; }
        public Dictionary<string, double> Models { get; set; }
    }
}
