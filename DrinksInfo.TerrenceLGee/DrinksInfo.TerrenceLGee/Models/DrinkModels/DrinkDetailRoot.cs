using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.DrinkModels;

public class DrinkDetailRoot
{
    [JsonPropertyName("drinks")]
    public List<DrinkDetail> DrinkDetails { get; set; } = [];
}
