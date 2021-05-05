using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_2.Models
{
    public class Ingredient
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Quantity { get; set; }
        [Required]
        public string Unit { get; set; }

        public override string ToString()
        {
            return $"{Name} {Quantity.ToString(CultureInfo.InvariantCulture.NumberFormat)} {Unit}";
        }
    }
}
