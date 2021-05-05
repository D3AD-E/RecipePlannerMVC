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
    public class RecipeController : Controller
    {

        // GET: Recipe/Details/5
        public ActionResult Details(string name)
        {
            return View();
        }

        // GET: Recipe/Create
        public ActionResult Create(bool force = false)
        {
            if (!force && HttpContext.Session.Keys.Contains("Recipe"))
            {
                var recipeData = JsonSerializer.Deserialize<Recipe>(HttpContext.Session.GetString("Recipe"));
                return View(recipeData);
            }
            Recipe recipe = new();
            HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
            return View(recipe);
        }

        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Recipe recipe, string command)
        {
            if (command == "Submit")
            {
                if (!ModelState.IsValid)
                {
                    return View(recipe);
                }

                await CheckRecipeAsync(recipe);

                if (ModelState.ErrorCount > 0)
                    return View(recipe);

                await RecipesService.AddAsync(recipe);
                ClearSession();

                TempData["Success"] = $"Successfully created recipe {recipe.Name}";
                return RedirectToAction(nameof(Index), "Home");
            }
            else if (command == "Add")
            {
                HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
                return AddIngredient();
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Recipe/Edit/5
        public async Task<ActionResult> EditAsync(string name, bool force=false)
        {
            if(!force && HttpContext.Session.Keys.Contains("RecipeName"))
            {
                ViewBag.RecipeName = HttpContext.Session.GetString("RecipeName");
                if(HttpContext.Session.Keys.Contains("Recipe"))
                {
                    var recipeData = JsonSerializer.Deserialize<Recipe>(HttpContext.Session.GetString("Recipe"));
                    return View(recipeData);
                }
            }

            var recipe = await RecipesService.GetRecipeAsync(name);
            if (recipe is not null)
            {
                HttpContext.Session.SetString("RecipeName", name);
                ViewBag.RecipeName = name;
                HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
                return View(recipe);
            }
            else
                return RedirectToAction(nameof(Index), "Home");
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Recipe recipe, string command)
        {
            if(command == "Submit")
            {
                if (!ModelState.IsValid)
                {
                    return View(recipe);
                }

                await CheckRecipeAsync(recipe);

                if (ModelState.ErrorCount > 0)
                    return View(recipe);

                await RecipesService.EditAsync(recipe, HttpContext.Session.GetString("RecipeName"));
                ClearSession();
                TempData["Success"] = $"Successfully edited recipe {recipe.Name}";
                return RedirectToAction(nameof(Index), "Home");
            }
            else if(command == "Add")
            {
                HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
                return AddIngredient();
            }
            else if(command == "Add")
            {
                HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
                return AddIngredient();
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        private async Task CheckRecipeAsync(Recipe recipe)
        {
            var previousName = HttpContext.Session.GetString("RecipeName");
            if (recipe.Name != previousName)
            {
                if (await RecipesService.ContainsAsync(recipe))
                    ModelState.AddModelError("Name", "Name already exists");
            }

            //mb linq could do it but I was not able to find solution
            Dictionary<string, bool> doubleNames = new();
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Name == "recipe")
                    ModelState.AddModelError("Ingredients", "Contains banned word 'recipe'");

                if (doubleNames.ContainsKey(ingredient.Name))
                {
                    if (!doubleNames[ingredient.Name])
                        doubleNames[ingredient.Name] = true;
                }
                else
                {
                    doubleNames.Add(ingredient.Name, false);
                }
            }

            foreach (var name in doubleNames)
            {
                if (name.Value)
                {
                    ModelState.AddModelError("Ingredients", $"Duplicate naming '{name.Key}'");
                }
            }
        }

        public ActionResult DeleteIngredient(int id)
        {
            if (!HttpContext.Session.Keys.Contains("Recipe"))
                return RedirectToAction(nameof(Index), "Home");

            var recipe = JsonSerializer.Deserialize<Recipe>(HttpContext.Session.GetString("Recipe"));
            if (recipe.Ingredients.Count > id)
            {
                recipe.Ingredients.RemoveAt(id);
                HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
            }

            if (HttpContext.Session.Keys.Contains("RecipeName"))
                return RedirectToAction("Edit");
            else
                return RedirectToAction("Create");
        }

        public ActionResult Cancel()
        {
            ClearSession();
            return RedirectToAction(nameof(Index), "Home");
        }

        private void ClearSession()
        {
            HttpContext.Session.Remove("RecipeName");
            HttpContext.Session.Remove("Recipe");
        }

        public ActionResult AddIngredient()
        {
            if(!HttpContext.Session.Keys.Contains("Recipe"))
                return RedirectToAction(nameof(Index), "Home");

            var recipe = JsonSerializer.Deserialize<Recipe>(HttpContext.Session.GetString("Recipe"));

            recipe.Ingredients.Add(new Ingredient());

            HttpContext.Session.SetString("Recipe", JsonSerializer.Serialize(recipe));
            if (HttpContext.Session.Keys.Contains("RecipeName"))
                return RedirectToAction("Edit");
            else
                return RedirectToAction("Create");
        }

        // GET: Recipe/Delete/5
        public async Task<ActionResult> DeleteAsync(string name)
        {
            await RecipesService.RemoveAsync(name);
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
