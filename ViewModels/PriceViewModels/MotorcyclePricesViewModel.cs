using MotoShop.DataTransferObjects;

namespace MotoShop.ViewModels.PriceViewModels
{
    public class MotorcyclePricesViewModel
    {
        public List<PricesByModelsDto> PricesByModels { get; set; }
        public Dictionary<string, double> PricesByTypes { get; set; }
        public Dictionary<string, double> PricesByBrands { get; set; }
    }
}
