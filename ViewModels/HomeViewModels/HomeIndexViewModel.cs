using MotoShop.Models;
using MotoShop.DataTransferObjects;

namespace MotoShop.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<MotorcycleDto> Motorcycles { get; set; }
    }
}
