using DrinksInfo.TerrenceLGee.Models.FilterModels.AlcoholicFilterModels;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;

public interface IAlcoholicService
{
    Task<List<Alcoholic>> GetAlcoholicsAsync();
}
