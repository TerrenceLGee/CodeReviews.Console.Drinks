using DrinksInfo.TerrenceLGee.Models.IngredientModels;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.IngredientServiceInterfaces;

public interface IIngredientDetailService
{
    Task<IngredientDetail?> GetIngredientDetailAsync(string ingredientName);
}
