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
    public class PaymentController : Controller
    {
        private readonly FarmerAdminContext _context;
        public PaymentController(FarmerAdminContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int PaymentID)
        {
            var paymentlist = from m in _context.Payment select m;
            return View(paymentlist);
        }
    }
}
