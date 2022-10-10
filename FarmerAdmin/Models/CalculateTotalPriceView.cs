using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class CalculateTotalPriceView
    {

        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Final Price")]
        public decimal FinalPrice { get; set; }
        
    }
}
