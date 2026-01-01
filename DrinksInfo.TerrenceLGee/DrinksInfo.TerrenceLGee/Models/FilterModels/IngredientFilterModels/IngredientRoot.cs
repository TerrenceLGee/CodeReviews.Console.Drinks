using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.IngredientFilterModels;

public class IngredientRoot
{
    [JsonPropertyName("drinks")]
    public List<Ingredient> Ingredients { get; set; } = [];
}
