using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.GlassFilterModels;

public class GlassRoot
{
    [JsonPropertyName("drinks")]
    public List<Glass> Glasses { get; set; } = [];
}
