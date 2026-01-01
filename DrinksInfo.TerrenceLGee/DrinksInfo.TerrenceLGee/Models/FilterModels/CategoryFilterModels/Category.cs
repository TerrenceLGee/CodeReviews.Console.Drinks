using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.CategoryFilterModels;

public class Category
{
    [JsonPropertyName("strCategory")]
    public string CategoryName { get; set; } = string.Empty;
}
