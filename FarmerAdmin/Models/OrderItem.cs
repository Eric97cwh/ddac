using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Display(Name = "ProductID")]
        public string ProductName { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "OrderID")]
        public int OrderID { get; set; }
    }
}
