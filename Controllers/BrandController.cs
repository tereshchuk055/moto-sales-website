using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.Repository;
using MotoShop.Models;
using MotoShop.ViewModels.BrandViewModels;
using MotoShop.DataTransferObjects;
using MotoShop.Constants;

namespace MotoShop.Controllers
{
    public class BrandController : Controller
    {
        private readonly BrandRepository _brandRepository;
        private readonly MotorcycleRepository _motorcycleRepository;

        public BrandController(BrandRepository brandRepository, MotorcycleRepository motorcycleRepository)
        {
            _brandRepository = brandRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        [HttpGet]
        public IActionResult Index([FromRoute] int id)
        {
            try
            {
                BrandIndexViewModel model = new()
                {
                    Brand = _brandRepository.GetById(id) ?? throw new Exception("Failed to find Brand!"),
                    Motorcycles = _motorcycleRepository.GetByBrandId(id),
                };

                return View(model);
            }
            catch{
                TempData[$"{DataResource.Error}"] = "Invalid Brand. Try again!";
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Create(Brand model)
        {
            if (ModelState.IsValid && _brandRepository.Create(model))
            {
                ViewData[$"{DataResource.Info}"] = "Brand created successfully!";
                return View();
            }
            return View(model);
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Settings()
        {
            BrandSettingsViewModel model = new()
            {
                Brands = _brandRepository.Get()
            };
            return View(model);
        }

        [HttpGet]
        [HasPrivilege("Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (_brandRepository.Delete(id))
            {
                TempData[$"{DataResource.Info}"] = "Brand was deleted successfully!";
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id)
        {
            Brand? model = _brandRepository.GetById(id);
            if (model is not null)
            {
                return View(model);
            }

            TempData[$"{DataResource.Error}"] = "Failed to find Brand!";
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id, Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (_brandRepository.GetById(id) != brand && _brandRepository.Update(brand))
                {
                    TempData[$"{DataResource.Info}"] = "Brand was updated successfully!";
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return View(brand);
            }
        }

        [Ajax]
        public JsonResult GetBrandsList()
        {
            List<Brand> data = _brandRepository.Get();
            return Json(data);
        }

        [Ajax]
        public JsonResult GetStats()
        {
            List<BrandStatsDto> data = _brandRepository.GetStats();

            return Json(data);
        }
    }
}
