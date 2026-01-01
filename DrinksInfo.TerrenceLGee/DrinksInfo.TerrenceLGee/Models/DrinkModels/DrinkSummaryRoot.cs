using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.DrinkModels;

public class DrinkSummaryRoot
{
    [JsonPropertyName("drinks")]
    public List<DrinkSummary> DrinkSummaries { get; set; } = [];
}
