using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Data.ActionFilters;
using MotoShop.Repository;
using MotoShop.ViewModels.OrderViewModels;
using MotoShop.Models;
using MotoShop.Constants;

namespace MotoShop.Controllers;

public class OrderController : Controller
{
    private readonly OrderRepository _orderRepository;
    private readonly MotorcycleRepository _motorcycleRepository;

    public OrderController(OrderRepository orderRepository, MotorcycleRepository motorcycleRepository)
    {
        _orderRepository = orderRepository;
        _motorcycleRepository = motorcycleRepository;
    }

    [Authorize]
    public IActionResult Index()
    {
        try
        {
            string? name = User.Identity?.Name;

            OrderIndexViewModel viewModel = new()
            {
                Orders = _orderRepository.GetOrders(name ?? throw new Exception("User is not authenticated!"))
            };

            return View(viewModel);
        }
        catch
        {
            TempData[$"{DataResource.Error}"] = "Invalid User. Try again!";
            return RedirectToAction("Index", "Home");
        }

    }

    [Ajax]
    public bool Create(string vin)
    {
        var user = User.Identity?.Name;
        if (_motorcycleRepository.IsMotorcycleVisible(vin) && user is not null)
        {
            Order model = new()
            {
                Customer = user,
                MotorcycleVIN = vin
            };
            _orderRepository.Create(model);
            return true;
        }
        return false;
    }

    [Ajax]
    [Authorize]
    public bool DeleteOrder(int id)
    {
        return _orderRepository.Delete(id);
    }

    [Ajax]
    [Authorize]
    public bool Confirm()
    {
        return _orderRepository.ConfirmOrder(User.Identity?.Name);
    }
}
