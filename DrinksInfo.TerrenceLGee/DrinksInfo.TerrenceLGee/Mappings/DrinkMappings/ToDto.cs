using DrinksInfo.TerrenceLGee.DTOs;
using DrinksInfo.TerrenceLGee.Models.DrinkModels;

namespace DrinksInfo.TerrenceLGee.Mappings.DrinkMappings;

public static class ToDto
{
    extension(DrinkDetail drink)
    {
        public DrinkDetailDto ToDrinkDetailDto()
        {
            return new DrinkDetailDto
            {
                DrinkId = drink.DrinkId,
                DrinkName = drink.DrinkName,
                Category = drink.Category,
                Alcoholic = drink.Alcoholic,
                Instructions = drink.Instructions,
                IngredientMeasures = drink.GetIngredientMeasures(),
            };
        }

        private List<string> GetIngredientMeasures()
        {
            var ingredients = drink.GetDrinkProperties("Ingredient");
            var measures = drink.GetDrinkProperties("Measure");

            var ingredientMeasures = new List<string>();

            foreach (var measure in measures.Zip(ingredients, Tuple.Create))
            {
                var ingredientMeasure = $"{measure.Item1.Trim()} of {measure.Item2.Trim()}";
                ingredientMeasures.Add(ingredientMeasure);
            }

            if (ingredients.Count <= measures.Count) return ingredientMeasures;

            for (var index = measures.Count; index < ingredients.Count; index++)
            {
                ingredientMeasures.Add(ingredients[index]);
            }

            return ingredientMeasures;
        }

        public List<string> GetDrinkProperties(string propertyName)
        {
            var propertyList = new List<string>();

            foreach (var property in drink.GetType().GetProperties())
            {
                var name = property.Name;
                if (property.Name.Contains($"{propertyName}"))
                {
                    var value = (string?)property.GetValue(drink);
                    if (!string.IsNullOrEmpty(value))
                    {
                        propertyList.Add(value);
                    }
                }
            }
            return propertyList;
        }
    }

    extension(DrinkSummary drink)
    {
        public DrinkSummaryDto ToDrinkSummaryDto()
        {
            return new DrinkSummaryDto
            {
                DrinkId = drink.DrinkId,
                DrinkName = drink.DrinkName
            };
        }
    }
}
