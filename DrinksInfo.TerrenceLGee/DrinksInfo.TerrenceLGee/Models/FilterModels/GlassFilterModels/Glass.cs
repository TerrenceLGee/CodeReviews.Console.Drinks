using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.GlassFilterModels;

public class Glass
{
    [JsonPropertyName("strGlass")]
    public string GlassName { get; set; } = string.Empty;
}
