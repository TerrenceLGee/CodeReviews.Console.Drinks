namespace DrinksInfo.TerrenceLGee.Tests.JsonResponses;

public static class AlcoholicResponse
{
    public static string GetAlcoholicResponse => """
                {
            "drinks": [
                {
                    "strAlcoholic": "Alcoholic"
                },
                {
                    "strAlcoholic": "Non alcoholic"
                },
                {
                    "strAlcoholic": "Optional alcohol"
                }
            ]
        }
        """;
}
