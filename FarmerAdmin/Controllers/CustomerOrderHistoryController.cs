using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Models;
using FarmerAdmin.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Models;
using FarmerAdmin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FarmerAdmin.Controllers
{
    public class CustomerOrderHistory : Controller
    {
        private const string bucketName = "farmerwebapplicationtp051172";
        private readonly FarmerAdminContext _context;
        public CustomerOrderHistory(FarmerAdminContext context)
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

        public async Task<IActionResult> Index(string msg = "")
        {
            var user = User.Identity.Name;
            var historyList = _context.CustomerOrderHistoryView.Where(m => m.UserName == user);
            return View(historyList);
        }
    }
}
