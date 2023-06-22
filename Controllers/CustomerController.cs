using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Repository;
using MotoShop.ViewModels.CustomerViewModels;
using MotoShop.Constants;

namespace MotoShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var customer = _customerRepository.GetByLogin(User?.Identity?.Name
                                ?? throw new Exception("Invalid User's Login!"));
                CustomerEditViewModel viewmodel = new()
                {
                    FirstName = customer?.FirstName,
                    LastName = customer?.LastName,
                    Email = customer?.Email,
                    Phone = customer?.Phone,
                };

                return customer is null ? throw new Exception("Invalid User's Login!") : View(viewmodel);
            }
            catch
            {
                TempData[$"{DataResource.Error}"] = "Invalid User. Try again!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(CustomerEditViewModel model)
        {
            try
            {
                model.Login = User?.Identity?.Name ?? throw new Exception("Invalid User's Login!");
                if (ModelState.IsValid && _customerRepository.Update(model))
                {
                    ViewData[$"{DataResource.Info}"] = "Profile was updated successfully!";
                }

                return View(model);
            }
            catch{
                TempData[$"{DataResource.Error}"] = "Invalid User. Try again!";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
