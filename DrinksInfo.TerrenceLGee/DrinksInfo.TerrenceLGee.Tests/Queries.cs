namespace DrinksInfo.TerrenceLGee.Tests;

public static class Queries
{
    public static string MockUrl => "https://www.example.com/api/";
    public static string ClientName => "client";
    public static string CategoryQuery => "filter.php?c=Cocktail";
    public static string GlassQuery => "filter.php?g=Champagne_Flute";
    public static string CategoryName => $"Cocktail";
    public static string GlassName => "Champagne_Flute";
    public static string IngredientQuery => "filter.php?i=Gin";
    public static string AlcoholicQuery => "filter.php?a=Non_Alcoholic";
    public static string AlcoholicName => "Non_Alcoholic";
    public static string RandomCategoryQuery => "random.php";
    public static string CategoriesQuery => "list.php?c=list";
    public static string IngredientDetailQuery => "search.php?i=Gin";
    public static string IngredientName => "Gin";
    public static string GlassesQuery => "list.php?g=list";
    public static string IngredientsQuery => "list.php?i=list";
    public static string AlcoholicsQuery => "list.php?a=list";
}
