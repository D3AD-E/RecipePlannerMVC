# RecipePlannerMVC

>University course project

    Please write simple Recipe Planner application using MVC NET 5.0
    Data should be stored in text files in selected folder (format explained at the end)

Main page

    Displays list of all available recipes
    Has buttons to Add/Edit/Delete recipes

Entry page

    user may Add new or Edit existing recipe
    recipe consist of multiline description and list of ingredients
    each ingredient has name, quantity and unit where:
    
    name is a multiword string (like bread flour)
    quantity is a float number (like 1.0)
    unit is a multiword string (any string is treated as a unit - so "red balbinka" is also a unit)

Menu page

    user may create a list of recipes to be cooked, for instance:

    omlette
    omlette
    pizza
    omlette

and program will compute sorted list of required ingredients to buy:

* active dry yeast: 0.25 ounce
* egg: 7.0 pcs
* flour: 2.5 cups
* olive oil:2.0 tablespoons
* salt: 3.0 pinch
* salt: 1.0 teaspoon
* water: 0.09 l

only compatible (=equal strings) units are summed up (see eggs with pcs) - if units are not compatible ingredients are presented separately (see salt)

Data format:

    recipes are stored in recipes.json file:
```json
{
"omlette": {
                "recipe": [
                    "Whisk eggs, water, salt and pepper.",
                    "Spray 8-inch (20 cm) non-stick skillet with cooking spray. ...",
                    "When eggs are almost set on surface but still look moist, cover half of omelette with filling. ...",
                    "Cook for a minute, then slide omelette onto plate.",
                    "(Nutrients per serving as per without filling)"
                ],
                "egg":"2.0 pcs ",
                "water": "0.03 l",
                "salt": "1.0 pinch"
            },
"pizza":    {
                "recipe": [
            "Preheat oven to 450 degrees F (230 degrees C).",
            "In a medium bowl, dissolve yeast and sugar in warm water.",
            "Let stand until creamy, about 10 minutes.",
            "Stir in flour, salt and oil. Beat until smooth. Let rest for 5 minutes.",
            "Turn dough out onto a lightly floured surface and pat or roll into a round.",
            "Transfer crust to a lightly greased pizza pan or baker's peel dusted with cornmeal.",
            "Spread with desired toppings and bake in preheated oven for 15 to 20 minutes, or until golden brown.",
            "Let baked pizza cool for 5 minutes before serving."
                ],
                "egg":"1.0 pcs",
                "flour": "2.5 cups",
                "salt": "1.0 teaspoon",
                "olive oil":"2.0 tablespoons",
                "active dry yeast": "0.25 ounce"
            }
}
```
## Screenshots
![Alt text](Pic1.png?raw=true)
![Alt text](Pic2.png?raw=true)
