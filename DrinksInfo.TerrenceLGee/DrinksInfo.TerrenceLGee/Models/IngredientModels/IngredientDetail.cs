using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.IngredientModels;

public class IngredientDetail
{
    [JsonPropertyName("idIngredient")]
    public string IngredientId { get; set; } = string.Empty;

    [JsonPropertyName("strIngredient")]
    public string IngredientName { get; set; } = string.Empty;

    [JsonPropertyName("strDescription")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("strType")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("strAlcohol")]
    public string Alcohol { get; set; } = string.Empty;

    [JsonPropertyName("strABV")]
    public string AlcoholByVolume { get; set; } = string.Empty; 
}
