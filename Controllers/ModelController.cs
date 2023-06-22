using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.Repository;
using MotoShop.Models;
using MotoShop.ViewModels.ModelViewModels;
using MotoShop.DataTransferObjects;
using MotoShop.Constants;

namespace MotoShop.Controllers
{
    public class ModelController : Controller
    {
        private readonly ModelRepository _modelRepository;

        public ModelController(ModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [HasPrivilege]
        public IActionResult Create(Model model)
        {
            if(ModelState.IsValid && _modelRepository.Create(model))
            {
                ViewData[$"{DataResource.Info}"] = "Model created successfully!";
                return View();
            }
            
            return View(model);

        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Settings()
        {
            ModelSettingsViewModel model = new()
            {
                Models = _modelRepository.Get()
            };
            
            return View(model);
        }

        [HttpGet]
        [HasPrivilege("Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (_modelRepository.Delete(id))
            {
                TempData[$"{DataResource.Info}"] = "Model was deleted successfully!";
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id)
        {
            if (_modelRepository.GetById(id) is not null)
            {
                return View(_modelRepository.GetById(id));
            }

            TempData[$"{DataResource.Error}"] = "Failed to find Model!";
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id, Model model)
        {
            if (ModelState.IsValid)
            {
                if (_modelRepository.GetById(id) != model && _modelRepository.Update(model))
                {
                    TempData[$"{DataResource.Info}"] = "Brand was updated successfully!";
                }
                return RedirectToAction("Dashboard", "Home");
            }

            return View(model);
        }

        [Ajax]
        public JsonResult GetModelsByBrand(int brandId)
        {
            List<Model> data = _modelRepository.GetByBrandId(brandId);
            return Json(data);
        }

        [Ajax]
        public JsonResult GetStats()
        {
            List<ModelStatsDto> data = _modelRepository.GetStats();
            return Json(data);
        }
    }
}
