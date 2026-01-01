using System.ComponentModel.DataAnnotations;

namespace DrinksInfo.TerrenceLGee.OptionEnums;

public enum DrinkSearchOptions
{
    [Display(Name = "By category")]
    Category,
    [Display(Name = "By ingredient")]
    Ingredient,
    [Display(Name = "By drink type")]
    Alcoholic,
    [Display(Name = "By glass served in")]
    Glass,
    [Display(Name = "By drink's first letter")]
    FirstLetter,
    [Display(Name = "By name/letters in the drink name")]
    SearchTerm,
    [Display(Name = "Get a random drink")]
    RandomDrink,
    [Display(Name = "Return to the previous menu")]
    Exit
}
