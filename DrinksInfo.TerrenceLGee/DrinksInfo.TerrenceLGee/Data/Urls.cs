namespace DrinksInfo.TerrenceLGee.Data;

public static class Urls
{
    public static string BaseUrl => "https://www.thecocktaildb.com/api/json/v1/1/";
    public static string CategoryListQuery => "list.php?c=list";
    public static string GlassesListQuery => "list.php?g=list";
    public static string IngredientsListQuery => "list.php?i=list";
    public static string AlcoholicListQuery => "list.php?a=list";
    public static string FilterDrinksByCategoryQuery => "filter.php?c=";
    public static string FilterDrinksByIngredientQuery => "filter.php?i=";
    public static string FilterDrinksByGlassQuery => "filter.php?g=";
    public static string FilterDrinksByAlcoholicQuery => "filter.php?a=";
    public static string SearchDrinksByFirstLetterQuery => "search.php?f=";
    public static string SearchDrinksByLettersQuery => "search.php?s=";
    public static string GetARandomDrinkQuery => "random.php";
    public static string LookupDrinkDetailsByDrinkIdQuery => "lookup.php?i=";
    public static string LookupIngredientByNameQuery => "search.php?i=";
}
