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
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;//appsettings.json section
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace FarmerAdmin.Controllers
{
    public class CartController : Controller
    {
        private const string topicARN = "arn:aws:sns:us-east-1:383278827480:TP038411Sns";
        private List<string> getKeysInformation()
        {
            List<string> values = new List<string>();
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfiguration configure = builder.Build(); //build the file

            values.Add(configure["Values:Value1"]);
            values.Add(configure["Values:Value2"]);
            values.Add(configure["Values:Value3"]);

            return values;
        }

        private const string bucketName = "farmerwebapplicationtp051172";
        private readonly FarmerAdminContext _context;
        public CartController(FarmerAdminContext context)
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

        public FarmerAdminContext Get_context()
        {
            return _context;
        }

        //instead of view all data, you can filter the data in same page

        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;

            var user = User.Identity.Name;
            var findUserId = _context.Users
              .Where(m => m.UserName == user)
              .Select(m => m.Id);
            var userId = findUserId.FirstOrDefault();
            var result = findUserId.ToListAsync().Result;

            var cartproductlist = from m 
                                  in _context.ProductCartView 
                                  where(m.UserId == userId)
                                  select m;

            
            ViewBag.UserId = String.Join(",", result);

            return View(cartproductlist);
        }

        public async Task<IActionResult> editCart(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var cart = await _context.Cart.FindAsync(ID);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editCart(int? ID, [Bind("CartId", "UserId, ProductId, Quantity, TotalPrice")] CustomerProduct cart)
        {
            if (cart.Quantity == 0)
            {
                var cartid = await _context.Cart.FindAsync(ID);
                _context.Cart.Remove(cartid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ID != cart.CartId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Problem in update, please check your input." + ex.Message);
                }
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index", "CartController");
                //return RedirectToAction("Index", "Cart", customerProduct);
            }
            return View(cart);
        }

        public IActionResult postPaymentOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> postPaymentOrder([Bind("PaymentMethod")] PaymentRequest paymentRequest)
        {
            var user = User.Identity.Name;
            var findUserId = _context.Users
              .Where(m => m.UserName == user)
              .Select(m => m.Id);
            var userId = findUserId.FirstOrDefault();
            var findUserRoleById = _context.UserRoles
              .Where(m => m.RoleId == Convert.ToString(1))
              .Select(m => m.UserId);
            var userRoleById = findUserRoleById.FirstOrDefault();

            var userDetail = _context.Users.Where(u => u.UserName == user).FirstOrDefault();
            var cartDetail = _context.Cart.Where(u => u.UserId == userId).FirstOrDefault();

            var query = _context.Cart.Where(x => x.UserId == userId);
            List<CustomerProduct> cartList = query.ToList();

            cartList.ForEach(m =>

            _context.Add(new CustomerOrder
            {
                ProductID = m.ProductId,
                OrderDate = DateTime.Now,
                Quantity = m.Quantity,
                UserName = user,
                Status = "Preparing"
            }
            )
            );
               var id = _context.SaveChanges();
            var OrderId = id;

            var cartQuery = _context.Cart.Where(x => x.UserId == userId);
            List<CustomerProduct> cartInfoList = cartQuery.ToList();

            cartList.ForEach(m =>
            _context.Remove(m)
            );
            _context.SaveChanges();

            var productDetail = _context.Product.Where(u => u.ProductID == cartDetail.ProductId).FirstOrDefault();

            var insertPayment =
            new Payment
            {
                PaymentMethod = paymentRequest.PaymentMethod,
                Amount = cartDetail.TotalPrice * cartDetail.Quantity,
                OrderID = OrderId
            };
            _context.Add(insertPayment);
            _context.SaveChanges();

            var PaymentId = insertPayment.PaymentID;
            var GetPaymentPrice = _context.CalculateTotalPriceView
              .Where(m => m.UserName == user)
              .Select(m => m.FinalPrice);
            var PaymentTotalPrice = GetPaymentPrice.FirstOrDefault();
            var financeData = _context.Finance.Where(c => c.FinanceID == 1).FirstOrDefault();

            financeData.Income = PaymentTotalPrice + financeData.Income;
            _context.Update(financeData);

            _context.SaveChanges();

            var broadcastText = $"Payment is successful, we will send item to you in 7 - 14 working days.For your reference: {OrderId}";
            List<string> values = getKeysInformation();
            var snsClient = new AmazonSimpleNotificationServiceClient(values[0], values[1], values[2], RegionEndpoint.USEast1);

            if (ModelState.IsValid)
            {
                try
                {
                    //add email as the subscriber
                    PublishRequest pubRequest = new PublishRequest(topicARN, broadcastText);
                    PublishResponse pubResponse = await snsClient.PublishAsync(pubRequest);
                }
                catch (AmazonSimpleNotificationServiceException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Unable to broadcast message to any email!");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
