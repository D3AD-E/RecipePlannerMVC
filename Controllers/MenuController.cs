using LAB_2.Core;
using LAB_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAB_2.Controllers
{
    public class MenuController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            MenuModel toShow = new();
            toShow.AllRecipes = await RecipesService.LoadRecipesAsync();
            if(HttpContext.Session.Keys.Contains("ChosenRecipes"))
            {
                var chosenRecipes = HttpContext.Session.GetString("ChosenRecipes");
                toShow.ChosenRecipes = JsonSerializer.Deserialize<List<string>>(chosenRecipes);
                //TempData["ChosenRecipes"] = chosenRecipes;//does not work otherwise could have used session

                foreach (var recipeName in toShow.ChosenRecipes)
                {
                    var recipe = await RecipesService.GetRecipeAsync(recipeName);
                    foreach(var ingredient in recipe.Ingredients)
                    {
                        toShow.AddIngredient(ingredient);
                    }
                }
            }
            toShow.SortIngredients();
            return View(toShow);
        }

        public IActionResult ChooseRecipe(string name)
        {
            //List<string> chosenRecipes = TempData["ChosenRecipes"] is null ? new() : JsonSerializer.Deserialize<List<string>>(TempData["ChosenRecipes"].ToString());3
            List<string> chosenRecipes = HttpContext.Session.Keys.Contains("ChosenRecipes") ? JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("ChosenRecipes")) : new();
            chosenRecipes.Add(name);

            //TempData["ChosenRecipes"] = JsonSerializer.Serialize(chosenRecipes);
            //TempData.Keep("ChosenRecipes");
            HttpContext.Session.SetString("ChosenRecipes", JsonSerializer.Serialize(chosenRecipes));
            return RedirectToAction("Index");
        }

        public IActionResult RemoveRecipe(string name)
        {
            if(!HttpContext.Session.Keys.Contains("ChosenRecipes"))
            {
                return RedirectToAction("Index");
            }

            List<string> chosenRecipes = JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("ChosenRecipes"));
            chosenRecipes.Remove(name);

            //TempData["ChosenRecipes"] = JsonSerializer.Serialize(chosenRecipes);
            HttpContext.Session.SetString("ChosenRecipes", JsonSerializer.Serialize(chosenRecipes));
            return RedirectToAction("Index");
        }
    }
}
