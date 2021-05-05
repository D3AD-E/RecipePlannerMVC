using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_2.Models
{
    public class MenuModel
    {
        public List<Recipe> AllRecipes { get; set; }
        public List<string> ChosenRecipes { get; set; }//store less data in temp
        public List<Ingredient> Ingredients { get;  private set; }

        public MenuModel()
        {
            AllRecipes = new();
            ChosenRecipes = new();
            Ingredients = new();
        }
        public void AddIngredient(Ingredient ingredient)
        {
            foreach (var currIngredint in Ingredients)
            {
                if(currIngredint.Name == ingredient.Name && currIngredint.Unit == ingredient.Unit)
                {
                    currIngredint.Quantity += ingredient.Quantity;
                    return;
                }
            }
            Ingredients.Add(ingredient);
        }

        public void SortIngredients()
        {
            Ingredients = Ingredients.OrderBy(x => x.Name).ToList();
        }
    }
}
