using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.Constants;
using MotoShop.Constants;
using MotoShop.Repository;
using MotoShop.ViewModels.AccountViewModels;
using MotoShop.ViewModels.CustomerViewModels;
using System.Security.Claims;

namespace MotoShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public AccountController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(CustomerSignUpViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid && _customerRepository.Create(model))
            {
                ViewData[$"{DataResource.Info}"] = "Account was created successfully!";
                return View();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(CustomerSignInViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var user = _customerRepository.Authenticate(model.Login, model.Password);

                if (user is not null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, model.Login),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ViewData[$"{DataResource.Error}"] = "Invalid Login or Password. Please, try again.";
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [HasPrivilege("Admin")]
        public IActionResult Settings()
        {
            AccountSettingsViewModel model = new()
            {
                Customers = _customerRepository.GetCustomers()
            };

            return View(model);
        }

        [Ajax]
        public string UpdateRole(string login)
        {
            try
            {
                UserRole result = _customerRepository.GetUserRoleByLogin(login);
                bool state = (result == UserRole.User) ?
                    _customerRepository.UpdateRole(UserRole.Moderator, login) :
                    _customerRepository.UpdateRole(UserRole.User, login);

                return (state) ? result.ToString() : throw new Exception("Failed to update User's Role");
            }
            catch{
                return "Failed";
            }
        }
    }
}
