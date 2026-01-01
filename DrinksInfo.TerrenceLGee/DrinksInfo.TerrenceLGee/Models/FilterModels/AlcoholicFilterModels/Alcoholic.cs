using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.AlcoholicFilterModels;

public class Alcoholic
{
    [JsonPropertyName("strAlcoholic")]
    public string AlcoholicName { get; set; } = string.Empty;
}
