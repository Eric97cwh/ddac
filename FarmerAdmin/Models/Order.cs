using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Order Address")]
        public string OrderAddress { get; set; }
        [Display(Name = "Order Status")]
        public int OrderStatus { get; set; }
    }
}
