using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class Finance
    {
        [Key]
        public int FinanceID { get; set; }

        [Display(Name = "Total Income")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Income { get; set; }
    }
}
