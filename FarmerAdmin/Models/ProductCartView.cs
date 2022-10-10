using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class ProductCartView
    {

        [Key]
        [Display(Name = "Cart ID")]
        public int CartId { get; set; }
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        [Display(Name = "Product Price")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Final Price")]
        public decimal FinalPrice { get; set; }
    }
}
