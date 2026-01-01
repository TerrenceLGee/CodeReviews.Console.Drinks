using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.CategoryFilterModels;

public class CategoryRoot
{
    [JsonPropertyName("drinks")]
    public List<Category> Categories { get; set; } = [];
}
