using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Models;
using FarmerAdmin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FarmerAdmin.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly FarmerAdminContext _context;
        public OrderItemController(FarmerAdminContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int ID)
        {
            var orderlist = from m in _context.OrderItem where m.OrderID == ID select m;
            return View(orderlist);
        }
    }
}
