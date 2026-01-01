using DrinksInfo.TerrenceLGee.Models.FilterModels.IngredientFilterModels;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;

public interface IIngredientService
{
    Task<List<Ingredient>> GetIngredientsAsync();
}
