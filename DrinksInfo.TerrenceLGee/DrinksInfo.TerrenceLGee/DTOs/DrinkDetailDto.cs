namespace DrinksInfo.TerrenceLGee.DTOs;

public class DrinkDetailDto
{
    public string DrinkId { get; set; } = string.Empty;
    public string DrinkName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Alcoholic { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public List<string> IngredientMeasures { get; set; } = [];
    public List<IngredientDetailDto> IngredientDetails { get; set; } = [];
}
