using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.DataTransferObjects;
using MotoShop.Repository;
using MotoShop.ViewModels.TypeViewModels;
using Type = MotoShop.Models.Type;
using MotoShop.Constants;

namespace MotoShop.Controllers
{
    public class TypeController : Controller
    {
        private readonly TypeRepository _typeRepository;
        
        public TypeController(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Create(Type model)
        {
            if (ModelState.IsValid && _typeRepository.Create(model))
            {
                ViewData[$"{DataResource.Info}"] = "Type created successfully!";
                return View();
            }
            return View(model);
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Settings() 
        {
            TypeSettingsViewModel model = new()
            {
                Types = _typeRepository.Get()
            };
            return View(model);
        }

        [HttpGet]
        [HasPrivilege("Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (_typeRepository.Delete(id))
            {
                TempData[$"{DataResource.Info}"] = "Type was deleted successfully!";
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id)
        {
            Type? model = _typeRepository.GetById(id);

            if (model is not null)
            {
                return View(model);
            }

            TempData[$"{DataResource.Error}"] = "Failed to find Type!";
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        [HasPrivilege]
        public IActionResult Edit([FromRoute] int id, Type type)
        {
            if(ModelState.IsValid)
            {
                if (_typeRepository.GetById(id) != type && _typeRepository.Update(type))
                {
                    TempData[$"{DataResource.Info}"] = "Type was updated successfully!";
                }
                return RedirectToAction("Dashboard", "Home");
            }

            return View(type);
        }

        [Ajax]
        public JsonResult GetTypeList()
        {
            List<Type> data = _typeRepository.Get();
            return Json(data);
        }

        [Ajax]
        public JsonResult GetStats()
        {
            List<TypeStatsDto> data = _typeRepository.GetStats();

            return Json(data);
        }
    }
}
