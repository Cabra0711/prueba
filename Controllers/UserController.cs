using Microsoft.AspNetCore.Mvc;
using PoliRiwi.Models;
using PoliRiwi.Services.Interfaces;

namespace PoliRiwi.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IPlaceServices _placeServices;
    private readonly IReservationService _reservationsService;

    public UserController(IUserService userService, IPlaceServices placeServices,
        IReservationService reservationService)
    {
        _userService = userService;
        _placeServices = placeServices;
        _reservationsService = reservationService;
    }
    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var response = await _userService.GetAllUsers();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(Users user)
    {
        
        // We enseure that the request of the time before
        // it arrives to de database as NULL
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        await _userService.CreateUser(user);
        return RedirectToAction("Users");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateUser(Users user)
    {
        user.UpdatedAt = DateTime.Now;
        await _userService.UpdateUser(user.Id,user);
        return RedirectToAction("Users");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);
        return RedirectToAction("Users");
    }
    [HttpPost]
    public async Task<IActionResult> CreateReservation(Places place)
    {
        
        place.CreatedAt = DateTime.Now;
        place.UpdatedAt = DateTime.Now;
        await _placeServices.CreatePlace(place);
        return RedirectToAction("Places");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPlaces()
    {
        var response = await _placeServices.GetAllPlaces();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlace(Places place)
    {
        
        place.CreatedAt = DateTime.Now;
        place.UpdatedAt = DateTime.Now;
        await _placeServices.CreatePlace(place);
        return RedirectToAction("Places");
    }
    public IActionResult Reservations()
    {
        return View();
    }

    public async Task<IActionResult> Places()
    {
        var response = await _placeServices.GetAllPlaces();
        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> UpdatePlace(Places place)
    {
        place.UpdatedAt = DateTime.Now;
        await _placeServices.UpdatePlace(place.Id,place);
        return RedirectToAction("Places");
    }
    
    [HttpPost]
    public async Task<IActionResult> DeletePlace(int id)
    {
        await _placeServices.DeletePlace(id);
        return RedirectToAction("Places");
    }
}