using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.IngredientFilterModels;

public class Ingredient
{
    [JsonPropertyName("strIngredient1")]
    public string IngredientName { get; set; } = string.Empty;
}
