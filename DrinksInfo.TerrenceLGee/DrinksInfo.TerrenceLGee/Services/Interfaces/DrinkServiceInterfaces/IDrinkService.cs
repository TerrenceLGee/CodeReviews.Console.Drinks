using DrinksInfo.TerrenceLGee.Models.DrinkModels;
using DrinksInfo.TerrenceLGee.OptionEnums;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.DrinkServiceInterfaces;

public interface IDrinkService
{
    Task<List<DrinkSummary>> GetDrinksAsync(DrinkSearchOptions option, string searchTerm);
    Task<DrinkDetail?> GetDrinkDetailAsync(string drinkId);
    Task<DrinkDetail?> GetRandomDrinkDetailAsync();
}
