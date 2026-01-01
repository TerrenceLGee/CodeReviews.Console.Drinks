using DrinksInfo.TerrenceLGee.DTOs;
using DrinksInfo.TerrenceLGee.Models.IngredientModels;

namespace DrinksInfo.TerrenceLGee.Mappings.IngredientMappings;

public static class ToDto
{
    extension(IngredientDetail ingredient)
    {
        public IngredientDetailDto ToIngredientDetailDto()
        {
            return new IngredientDetailDto
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                Description = ingredient.Description,
                Type = ingredient.Type,
                Alcohol = ingredient.Alcohol,
                AlcoholByVolume = ingredient.AlcoholByVolume,
            };
        }
    }
}
