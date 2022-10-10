using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmerAdmin.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmerAdmin.Data
{
    public class FarmerAdminContext : IdentityDbContext<User>
    {
        public FarmerAdminContext(DbContextOptions<FarmerAdminContext> options)
            : base(options)
        {
        }

        public DbSet<FarmerAdmin.Models.Product> Product { get; set; }
        public DbSet<FarmerAdmin.Models.Finance> Finance { get; set; }
        public DbSet<FarmerAdmin.Models.CustomerOrder> CustomerOrder { get; set; }
        public DbSet<FarmerAdmin.Models.OrderItem> OrderItem { get; set; }
        public DbSet<FarmerAdmin.Models.Payment> Payment { get; set; }
        public DbSet<FarmerAdmin.Models.CustomerProduct> Cart { get; set; }
        public DbSet<FarmerAdmin.Models.ProductCartView> ProductCartView { get; set; }
        public DbSet<FarmerAdmin.Models.CalculateTotalPriceView> CalculateTotalPriceView { get; set; }
        public DbSet<FarmerAdmin.Models.CustomerOrderHistoryView> CustomerOrderHistoryView { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
