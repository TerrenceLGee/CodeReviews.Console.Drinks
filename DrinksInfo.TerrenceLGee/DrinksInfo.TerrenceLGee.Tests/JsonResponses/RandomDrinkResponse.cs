namespace DrinksInfo.TerrenceLGee.Tests.JsonResponses;

public static class RandomDrinkResponse
{
    public static string GetRandomDrinkResponse => """
                {
            "drinks": [
                {
                    "idDrink": "11117",
                    "strDrink": "Blue Lagoon",
                    "strCategory": "Ordinary Drink",
                    "strAlcoholic": "Alcoholic",
                    "strGlass": "Highball glass",
                    "strInstructions": "Pour vodka and curacao over ice in a highball glass. Fill with lemonade, top with the cherry, and serve.",
                    "strIngredient1": "Vodka",
                    "strIngredient2": "Blue Curacao",
                    "strIngredient3": "Lemonade",
                    "strIngredient4": "Cherry",
                    "strMeasure1": "1 oz ",
                    "strMeasure2": "1 oz "
                }
            ]
        }
        """;
}
