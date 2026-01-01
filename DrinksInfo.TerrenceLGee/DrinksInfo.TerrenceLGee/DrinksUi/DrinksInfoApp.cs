using DrinksInfo.TerrenceLGee.DrinksUi.Helpers;
using DrinksInfo.TerrenceLGee.DrinksUi.Interfaces;
using DrinksInfo.TerrenceLGee.Extensions;
using DrinksInfo.TerrenceLGee.OptionEnums;
using Spectre.Console;

namespace DrinksInfo.TerrenceLGee.DrinksUi;

public class DrinksInfoApp
{
    private readonly IDrinksUi _drinksUi;

    public DrinksInfoApp(IDrinksUi drinksUi) => _drinksUi = drinksUi;

    public async Task Run()
    {
        DisplayMessage("Drinks Info App");
        UiHelpers.PressAnyKeyToContinue();

        var userFinished = false;

        while (!userFinished)
        {
            var choice = GetUserChoice();

            switch (choice)
            {
                case MainMenuOptions.SearchForDrinks:
                    await _drinksUi.Run();
                    break;
                case MainMenuOptions.Exit:
                    userFinished = true;
                    break;
            }
        }

        DisplayMessage("Goodbye");
    }

    private static MainMenuOptions GetUserChoice()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
            .Title("[CadetBlue]Please choose one of the following options[/]")
            .AddChoices(Enum.GetValues<MainMenuOptions>())
            .UseConverter(choice => choice.GetDisplayName()));
    }

    private static void DisplayMessage(string message)
    {
        AnsiConsole.Write(
            new FigletText($"{message}")
            .Centered()
            .Color(Color.Aquamarine3));
    }
}
