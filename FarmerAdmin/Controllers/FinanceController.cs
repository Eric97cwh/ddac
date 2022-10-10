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
    public class FinanceController : Controller
    {
        private readonly FarmerAdminContext _context;
        public FinanceController(FarmerAdminContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int FinanceID)
        {
            var financelist = from m in _context.Finance select m;
            return View(financelist);
        }
    }
}
