using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.DrinkModels;

public class DrinkSummary
{
    [JsonPropertyName("idDrink")]
    public string DrinkId { get; set; } = string.Empty;

    [JsonPropertyName("strDrink")]
    public string DrinkName { get; set; } = string.Empty;
}
