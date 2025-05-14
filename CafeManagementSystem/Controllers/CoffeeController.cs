using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,User")] // Both Admins and Users can access
    public class CoffeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")] // Only Admins can create coffee
        public IActionResult Create()
        {
            return View();
        }
    }
}