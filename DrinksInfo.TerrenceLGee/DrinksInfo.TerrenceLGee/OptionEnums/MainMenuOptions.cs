using System.ComponentModel.DataAnnotations;

namespace DrinksInfo.TerrenceLGee.OptionEnums;

public enum MainMenuOptions
{
    [Display(Name = "Search for drinks")]
    SearchForDrinks,
    [Display(Name = "Exit the program")]
    Exit
}
