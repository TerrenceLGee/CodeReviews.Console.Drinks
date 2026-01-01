using System.Text.Json.Serialization;

namespace DrinksInfo.TerrenceLGee.Models.FilterModels.AlcoholicFilterModels;

public class AlcoholicRoot
{
    [JsonPropertyName("drinks")]
    public List<Alcoholic> Alcoholics { get; set; } = [];
}
