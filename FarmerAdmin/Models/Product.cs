using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmerAdmin.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; }
        [Display(Name = "Product Picture")]
        public string ProductPicture { get; set; }
        [Display(Name = "Product Picture Name")]
        public string ProductPictureName { get; set; }
    }
}
