using DrinksInfo.TerrenceLGee.DTOs;
using Spectre.Console;

namespace DrinksInfo.TerrenceLGee.DrinksUi.Helpers;

public static class ViewHelpers
{
    public static void DisplayDrinkSummaries(List<DrinkSummaryDto> drinks)
    {
        AnsiConsole.WriteLine();

        var table = new Table()
            .BorderColor(Color.DarkBlue);
        table.AddColumn("[LightSteelBlue1]Drink Id[/]");
        table.AddColumn("[Aquamarine1]Drink Name[/]");

        foreach (var drink in drinks)
        {
            table.AddRow(
                $"[LightSteelBlue1]{drink.DrinkId}[/]",
                $"[Aquamarine1]{drink.DrinkName}[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayIngredientDetails(List<IngredientDetailDto> ingredients)
    {
        AnsiConsole.WriteLine();

        var table = new Table()
            .BorderColor(Color.BlueViolet);
        table.AddColumn("[Cyan1]Name[/]");
        table.AddColumn("[Cyan1]Description[/]");
        table.AddColumn("[Cyan1]Type[/]");
        table.AddColumn("[Cyan1]Is Alchoholic?[/]");
        table.AddColumn("[Cyan1]Alcohol By Volume[/]");

        foreach (var ingredient in ingredients)
        {
            table.AddRow(
                $"[DarkOliveGreen1_1]{ingredient.IngredientName ?? "N/A"}[/]",
                $"[DarkOliveGreen1_1]{ingredient.Description ?? "N/A"}[/]",
                $"[DarkOliveGreen1_1]{ingredient.Type ?? "N/A"}[/]",
                $"[DarkOliveGreen1_1]{ingredient.Alcohol ?? "N/A"}[/]",
                $"[DarkOliveGreen1_1]{ingredient.AlcoholByVolume ?? "N/A"}[/]");
        }

        AnsiConsole.Write(table);
    }
}
