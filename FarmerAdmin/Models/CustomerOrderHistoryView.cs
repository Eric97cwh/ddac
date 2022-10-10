using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class CustomerOrderHistoryView
    {
        [Key]
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }

        [Display(Name = "Order Item ID")]
        public int OrderItemID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Product Price")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}
