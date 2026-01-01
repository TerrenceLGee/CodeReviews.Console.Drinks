using DrinksInfo.TerrenceLGee.Models.FilterModels.GlassFilterModels;

namespace DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;

public interface IGlassService
{
    Task<List<Glass>> GetGlassesAsync();
}
