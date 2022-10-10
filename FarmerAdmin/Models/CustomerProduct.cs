using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class CustomerProduct
    {
        [Key]
        public int CartId { get; set; }

        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

    }
}
