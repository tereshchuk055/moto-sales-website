using MotoShop.Models;
using MotoShop.DataTransferObjects;

namespace MotoShop.ViewModels.BrandViewModels
{
    public class BrandIndexViewModel
    {
        public Brand Brand { get; set; }
        public List<MotorcycleDto> Motorcycles { get; set; }
    }
}
