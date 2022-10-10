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
    public class OrderController : Controller
    {
        private readonly FarmerAdminContext _context;
        public OrderController(FarmerAdminContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int OrderID)
        {
            var orderlist = from m in _context.CustomerOrder select m;
            return View(orderlist);
        }

        //funtion - edit and update
        public async Task<IActionResult> UpdateStatus(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var order = await _context.CustomerOrder.FindAsync(ID);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int? ID, [Bind("OrderID", "UserName", "OrderDate", "OrderAddress", "OrderStatus")] CustomerOrder Order)
        {
            if (ID != Order.OrderID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Unable to update the order status of " + Order.OrderID + ". Error: " + ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Order);
        }

        //funtion - edit and update
        public async Task<IActionResult> ViewItem(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "OrderItem", new { ID = ID });
        }
    }
}
