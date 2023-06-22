using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.Repository;
using MotoShop.ViewModels;
using MotoShop.ViewModels.HomeViewModels;
using MotoShop.Constants;
using System.Diagnostics;

namespace MotoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly BrandRepository _brandRepository;
        private readonly MotorcycleRepository _motorcycleRepository;

        public HomeController(BrandRepository brandRepository, MotorcycleRepository motorcycleRepository)
        {
            _brandRepository = brandRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel viewModel = new()
            {
                Brands = _brandRepository.GetRandomTen(),
                Motorcycles = _motorcycleRepository.GetFiveNewest()
            };

            ViewData[$"{DataResource.Error}"] = TempData[$"{DataResource.Error}"]?.ToString();
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Dashboard()
        {
            ViewData[$"{DataResource.Info}"] = TempData[$"{DataResource.Info}"]?.ToString();
            ViewData[$"{DataResource.Error}"] = TempData[$"{DataResource.Error}"]?.ToString();
            return View();
        }

        [HasPrivilege("Admin")]
        public IActionResult Backup()
        {
            string folderPath = $"{Directory.GetCurrentDirectory()}\\Temp\\";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            if (_motorcycleRepository.CreateBackup())
            {
                string path = $"{Directory.GetCurrentDirectory()}\\Temp\\MotoShop_FullDbBkup.bak";
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }

            TempData[$"{DataResource.Error}"] = "Failed to create backup!";
            return RedirectToAction("Dashboard", "Home");
        }
    }
}