using Microsoft.AspNetCore.Mvc;

namespace FarmerAdmin.Controllers
{
    public class OrderHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
