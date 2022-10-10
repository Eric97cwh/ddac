using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Models;
using FarmerAdmin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
//for linking your AWS account
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;//appsettings.json section
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace FarmerAdmin.Controllers
{
    public class CustomerController : Controller
    {
        private const string bucketName = "farmerwebapplicationtp051172";
        private readonly FarmerAdminContext _context;
        public CustomerController(FarmerAdminContext context)
        {
            _context = context;
        }
        //function 1: connection string to the AWS Account
        private List<string> getValues()
        {
            List<string> values = new List<string>();
            //link to the appsettings.json and get back the values
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();
            values.Add(configure["Values:Value1"]);
            values.Add(configure["Values:Value2"]);
            values.Add(configure["Values:Value3"]);
            return values;
        }
        //instead of view all data, you can filter the data in same page

        public async Task<IActionResult> Index(string ProductName, string msg = "")
        {
            ViewBag.msg = msg;

            

            var productlist = from m in _context.Product select m;
            if (!string.IsNullOrEmpty(ProductName))
            {
                productlist = productlist.Where(s => s.ProductName.Contains(ProductName));
            }

            var user = User.Identity.Name;
            var findUserId = _context.Users
              .Where(m => m.UserName == user)
              .Select(m => m.Id);
            var result = findUserId.ToListAsync().Result;
            ViewBag.UserId = String.Join(",", result);

            return View(productlist);
        }

        public IActionResult AddCart()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCart([Bind("UserId,ProductId,Quantity,TotalPrice")] CustomerProduct customerProduct)
        {
            var user = User.Identity.Name;
            var findUserId = _context.Users
              .Where(m => m.UserName == user)
              .Select(m => m.Id);
            var userId = findUserId.FirstOrDefault();

            var existing = _context.Cart.Where(c => c.ProductId == customerProduct.ProductId && c.UserId == userId).FirstOrDefault();

            if (existing == null)
            {
                // not existing found, insert new cart
                customerProduct.UserId = userId;
                _context.Add(customerProduct);
            }
            else
            {
                // update existing
                existing.Quantity = existing.Quantity + customerProduct.Quantity;
                _context.Update(existing);
            }

            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));

        }
    }
}
