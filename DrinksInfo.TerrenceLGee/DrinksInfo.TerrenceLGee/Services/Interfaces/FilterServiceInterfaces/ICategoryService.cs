using DrinksInfo.TerrenceLGee.Models.FilterModels.CategoryFilterModels;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;

public interface ICategoryService
{
    Task<List<Category>> GetCategoriesAsync();
}
