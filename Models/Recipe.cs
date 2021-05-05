using LAB_2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_2.Models
{
    public class Recipe
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe()
        {
            Ingredients = new();
        }
    }
}
