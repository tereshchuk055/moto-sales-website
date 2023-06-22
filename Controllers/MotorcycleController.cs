using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.ViewModels.MotorcycleViewModels;
using MotoShop.Repository;
using MotoShop.DataTransferObjects;
using MotoShop.Data.ActionFilters;
using MotoShop.Services;
using MotoShop.Models;
using MotoShop.ViewModels.PriceViewModels;
using MotoShop.Constants;

namespace MotoShop.Controllers
{
    public class MotorcycleController : Controller
    {
        private readonly MotorcycleRepository _motorcycleRepository;
        private readonly BrandRepository _brandRepository;
        private readonly ModelRepository _modelRepository;
        private readonly TypeRepository _typeRepository;
        private readonly IPhotoService _photoService;

        public MotorcycleController(MotorcycleRepository motorcycleRepository, BrandRepository brandRepository, ModelRepository modelRepository, TypeRepository typeRepository, IPhotoService photoService)
        {
            _motorcycleRepository = motorcycleRepository;
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
            _typeRepository = typeRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Create(MotorcycleCreateViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var result = _photoService.AddPhotoAsync(model.Photo);

                bool state = _motorcycleRepository.Create(new Motorcycle
                {
                    VIN = model.VIN,
                    ModelId = model.ModelId,
                    TypeId = model.TypeId,
                    EngineDisplacement = model.EngineDisplacement,
                    Price = model.Price,
                    Manufactured = model.Manufactured,
                    PhotoPath = result.Result.Url.ToString(),
                }); 

                if(state)
                    ViewData[$"{DataResource.Info}"] = "Motorcycle created successfully!";
            }
            return View(model);
        }
        
        [HttpGet]
        [HasPrivilege]
        public IActionResult Settings()
        {
            MotorcycleSettingsViewModel model = new()
            {
                Motorcycles = _motorcycleRepository.Get()
            };

            return View(model);
        }

        [HttpGet]
        [HasPrivilege("Admin")]
        public IActionResult Delete([FromRoute] string id)
        {
            if (_motorcycleRepository.Delete(id))
            {
                TempData[$"{DataResource.Info}"] = "Motorcycle was deleted successfully!";
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] string id)
        {
            MotorcycleEditViewModel? viewModel = _motorcycleRepository.GetEditVMById(id);

            if (viewModel is not null)
            {
                return View(viewModel);
            }
            else
            {
                TempData[$"{DataResource.Error}"] = "Failed to find Motorcycle!";
                return RedirectToAction("Dashboard", "Home");
            }
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] string id, MotorcycleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var editModel = _motorcycleRepository.GetById(id);
                string? path = editModel?.PhotoPath;

                if(model.Photo is not null) 
                {
                    if(!string.IsNullOrEmpty(editModel?.PhotoPath))
                    {
                        _photoService.DeletePhotoAsync(editModel.PhotoPath);
                    }
                    path = _photoService.AddPhotoAsync(model.Photo).Result.Url.ToString();
                }

                model.VIN = id;
                editModel = new(model, path ?? "~/images/Silhouette.jpg");

                if (_motorcycleRepository.Update(editModel))
                {
                    TempData[$"{DataResource.Info}"] = "Motorcycle was updated successfully!";
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public IActionResult Prices() 
        {
            var pricesByModels = new List<PricesByModelsDto>();

            foreach (var brand in _brandRepository.Get())
            {
                var models = _modelRepository.GetModelPriceDictionary(brand.BrandName);
                if(models is not null)
                    pricesByModels.Add(new PricesByModelsDto()
                    {
                        BrandName = brand.BrandName,
                        Models = models
                    });
            }

            MotorcyclePricesViewModel model = new()
            {
                PricesByModels = pricesByModels,
                PricesByBrands = _brandRepository.GetBrandsAveragePrices(),
                PricesByTypes = _typeRepository.GetTypesAveragePrices(),
            };
            return View(model);
        }
        
        [Ajax]
        public JsonResult GetNextEight(int number) 
        {
            List<MotorcycleDto> data = _motorcycleRepository.GetNextEight(number);
            return Json(data);
        } 
        
        [Ajax]
        public JsonResult Search(string value) 
        {
            List<MotorcycleDto> data = _motorcycleRepository.Search(value);
            return Json(data);
        }
    }
}
