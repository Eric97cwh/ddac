using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Display(Name = "Payment Method")]
        public int PaymentMethod { get; set; }
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
    }
}
