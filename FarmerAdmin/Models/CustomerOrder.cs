using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class CustomerOrder
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "User ID")]
        public string UserID { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
