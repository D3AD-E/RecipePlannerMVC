using LAB_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Text.Json;
using System.Globalization;
using System.Text;

namespace LAB_2.Core
{
    //it would be better not to rewrite whole file each time
    public static class RecipesService
    {
        const string FILE_NAME = "recipes.json";

        public static async Task EditAsync(Recipe recipe, string nameKey)
        {
            var json =  await ReadJsonAsync();
            if(json.ContainsKey(nameKey))
            {
                var recipeJson = ToJson(recipe);
                if (nameKey == recipe.Name)
                {
                    json[nameKey] = recipeJson.Value;
                }
                else
                {
                    json.Remove(nameKey);
                    json.Add(recipeJson.Key, recipeJson.Value);
                }
                await SaveJsonAsync(json);
            }
        }
        public static async Task RemoveAsync(Recipe recipe)
        {
            var json = await ReadJsonAsync();
            if (json.ContainsKey(recipe.Name))
            {
                json.Remove(recipe.Name);
                await SaveJsonAsync(json);
            }
        }
        public static async Task RemoveAsync(string name)
        {
            var json = await ReadJsonAsync();
            if (json.ContainsKey(name))
            {
                json.Remove(name);
                await SaveJsonAsync(json);
            }
        }
        public static async Task AddAsync(Recipe recipe)
        {
            var json = await ReadJsonAsync();
            if (!json.ContainsKey(recipe.Name))
            {
                var recipeJson = ToJson(recipe);
                json.Add(recipeJson.Key, recipeJson.Value);
                await SaveJsonAsync(json);
            }
        }

        public static async Task<List<Recipe>> LoadRecipesAsync()
        {
            var json = await ReadJsonAsync();

            List<Recipe> recipes = new();

            foreach (var recipeJson in json)
            {

                Recipe recipe = new Recipe
                {
                    Name = recipeJson.Key,
                    Ingredients = new()
                };

                var recipeInternalJson = JsonSerializer.Deserialize<Dictionary<string, object>>(recipeJson.Value.ToString());

                foreach (var ingredientJson in recipeInternalJson)
                {
                    if (ingredientJson.Key == "recipe")
                    {
                        var description = JsonSerializer.Deserialize<List<string>>(ingredientJson.Value.ToString());
                        recipe.Description = string.Join(Environment.NewLine, description);
                    }
                    else
                    {
                        var ingredientDesciption = ingredientJson.Value.ToString().Split(' ', 2);

                        Ingredient ingredient = new Ingredient
                        {
                            Name = ingredientJson.Key,
                            Quantity = float.Parse(ingredientDesciption[0], CultureInfo.InvariantCulture.NumberFormat),
                            Unit = ingredientDesciption[1]
                        };
                        recipe.Ingredients.Add(ingredient);
                    }
                }
                recipes.Add(recipe);
            }

            return recipes;
        }

        public static async Task<Recipe> GetRecipeAsync(string name)
        {
            var json = await ReadJsonAsync();

            foreach (var recipeJson in json)
            {
                if (recipeJson.Key != name)
                    continue;
                Recipe recipe = new Recipe
                {
                    Name = recipeJson.Key,
                    Ingredients = new()
                };

                var recipeInternalJson = JsonSerializer.Deserialize<Dictionary<string, object>>(recipeJson.Value.ToString());

                foreach (var ingredientJson in recipeInternalJson)
                {
                    if (ingredientJson.Key == "recipe")
                    {
                        var description = JsonSerializer.Deserialize<List<string>>(ingredientJson.Value.ToString());
                        recipe.Description = string.Join(Environment.NewLine, description);
                    }
                    else
                    {
                        var ingredientDesciption = ingredientJson.Value.ToString().Split(' ', 2);

                        Ingredient ingredient = new Ingredient
                        {
                            Name = ingredientJson.Key,
                            Quantity = float.Parse(ingredientDesciption[0], CultureInfo.InvariantCulture.NumberFormat),
                            Unit = ingredientDesciption[1]
                        };
                        recipe.Ingredients.Add(ingredient);
                    }
                }
                return recipe;
            }

            return null;
        }
        public static async Task SaveRecipesAsync(List<Recipe> recipes)
        {
            var recipesJson = new Dictionary<string, object>();
            foreach (var recipe in recipes)
            {
                var recipeJson = ToJson(recipe);
                recipesJson.Add(recipeJson.Key, recipeJson.Value);
            }
            await SaveJsonAsync(recipesJson);
        }
        public static async Task<bool> ContainsAsync(Recipe recipe)
        {
            var json = await ReadJsonAsync();
            return json.ContainsKey(recipe.Name);
        }
        public static async Task<bool> ContainsAsync(string name)
        {
            var json = await ReadJsonAsync();
            return json.ContainsKey(name);
        }

        private static async Task<Dictionary<string, object>> ReadJsonAsync()
        {
            using FileStream readStream = File.OpenRead(FILE_NAME);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(readStream);
        }

        private static async Task SaveJsonAsync(Dictionary<string, object> jsonDocument, bool beautify = true)
        {
            using FileStream createStream = File.Create(FILE_NAME);
            if(beautify)
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                await JsonSerializer.SerializeAsync(createStream, jsonDocument, jsonSerializerOptions);
            }
            else
            {
                await JsonSerializer.SerializeAsync(createStream, jsonDocument);
            }
        }

        private static RecipeJson ToJson(Recipe recipe)
        {
            Dictionary<string, object> ingredientsJson = new();

            var descriptionJson = recipe.Description.Split(Environment.NewLine);
            ingredientsJson.Add("recipe", descriptionJson);

            foreach (var ingredient in recipe.Ingredients)
            {
                ingredientsJson.Add(ingredient.Name, $"{ingredient.Quantity.ToString(CultureInfo.InvariantCulture.NumberFormat)} {ingredient.Unit}");
            }

            return new RecipeJson() { Key = recipe.Name, Value = ingredientsJson };
        }
    }

}
