using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.IngredientModels;

public class IngredientDetailRoot
{
    [JsonPropertyName("ingredients")]
    public List<IngredientDetail> IngredientDetails { get; set; } = [];
}
