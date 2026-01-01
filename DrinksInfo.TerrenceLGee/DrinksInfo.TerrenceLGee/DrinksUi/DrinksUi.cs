using DrinksInfo.TerrenceLGee.DrinksUi.Helpers;
using DrinksInfo.TerrenceLGee.DrinksUi.Interfaces;
using DrinksInfo.TerrenceLGee.DTOs;
using DrinksInfo.TerrenceLGee.Extensions;
using DrinksInfo.TerrenceLGee.Mappings.DrinkMappings;
using DrinksInfo.TerrenceLGee.Mappings.IngredientMappings;
using DrinksInfo.TerrenceLGee.Models.DrinkModels;
using DrinksInfo.TerrenceLGee.Models.FilterModels.AlcoholicFilterModels;
using DrinksInfo.TerrenceLGee.Models.FilterModels.CategoryFilterModels;
using DrinksInfo.TerrenceLGee.Models.FilterModels.GlassFilterModels;
using DrinksInfo.TerrenceLGee.Models.FilterModels.IngredientFilterModels;
using DrinksInfo.TerrenceLGee.OptionEnums;
using DrinksInfo.TerrenceLGee.Services.Interfaces.DrinkServiceInterfaces;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using DrinksInfo.TerrenceLGee.Services.Interfaces.IngredientServiceInterfaces;
using Spectre.Console;

namespace DrinksInfo.TerrenceLGee.DrinksUi;

public class DrinksUi : IDrinksUi
{
    private readonly ICategoryService _categoryService;
    private readonly IIngredientService _ingredientService;
    private readonly IAlcoholicService _alcoholicService;
    private readonly IGlassService _glassService;
    private readonly IDrinkService _drinkService;
    private readonly IIngredientDetailService _ingredientDetailService;

    public DrinksUi(
        ICategoryService categoryService,
        IIngredientService ingredientService,
        IAlcoholicService alcoholicService,
        IGlassService glassService,
        IDrinkService drinkService,
        IIngredientDetailService ingredientDetailService)
    {
        _categoryService = categoryService;
        _ingredientService = ingredientService;
        _alcoholicService = alcoholicService;
        _glassService = glassService;
        _drinkService = drinkService;
        _ingredientDetailService = ingredientDetailService;
    }

    public async Task Run()
    {
        var userFinished = false;

        while (!userFinished)
        {
            var drinks = new List<DrinkSummary>();
            var drinkSummaries = new List<DrinkSummaryDto>();
            var drinkId = string.Empty;
            var choice = GetDrinkSearchChoice();

            switch (choice)
            {
                case DrinkSearchOptions.Category:
                    var category = await GetCategoryNameAsync();
                    await SearchDrinksAsync(choice, category);
                    break;
                case DrinkSearchOptions.Ingredient:
                    var ingredient = await GetIngredientNameAsync();
                    await SearchDrinksAsync(choice, ingredient);
                    break;
                case DrinkSearchOptions.Alcoholic:
                    var alcoholic = await GetAlcoholicNameAsync();
                    await SearchDrinksAsync(choice, alcoholic);
                    break;
                case DrinkSearchOptions.Glass:
                    var glass = await GetGlassNameAsync();
                    await SearchDrinksAsync(choice, glass);
                    break;
                case DrinkSearchOptions.FirstLetter:
                    var firstLetter = AnsiConsole
                        .Ask<string>("[DarkOliveGreen3_2]Enter the first letter of the drinks that you wish to view [/]");
                    Console.Clear();
                    await SearchDrinksAsync(choice, firstLetter);
                    break;
                case DrinkSearchOptions.SearchTerm:
                    var searchTerm = AnsiConsole
                        .Ask<string>("[LightYellow3]Enter a search term (i.e. 'et') " +
                                    "to find all drinks containing those consecutive letters : [/]");
                    Console.Clear();
                    drinks = await _drinkService.GetDrinksAsync(DrinkSearchOptions.SearchTerm, searchTerm);
                    await SearchDrinksAsync(choice, searchTerm);
                    break;
                case DrinkSearchOptions.RandomDrink:
                    var randomDrink = await _drinkService.GetRandomDrinkDetailAsync();
                    if (randomDrink is not null)
                    {
                        drinkId = randomDrink.DrinkId;
                        await ViewDrinkDetails(drinkId);
                    }
                    else
                    {
                        UiHelpers
                            .PressAnyKeyToContinueError("Unable to retrieve a random drink at this time");
                    }
                    break;
                case DrinkSearchOptions.Exit:
                    userFinished = true;
                    break;
                default:
                    UiHelpers
                        .PressAnyKeyToContinueError("Invalid option chosen\nReturning to previous menu");
                    break;
            }
        }
    }

    private static string ViewDrinksAndGetDrinkId(List<DrinkSummaryDto> drinks)
    {
        var pageSize = 10;
        var changePageSize = AnsiConsole.Confirm($"[DarkSlateGray3]There are {drinks.Count} " +
            $"drinks for display and currently you can view {pageSize} drinks per page" +
            $"\nWould you like to specify how many drinks you can view per page? [/]");

        if (changePageSize)
        {
            pageSize = AnsiConsole.Ask<int>("[DarkSlateGray3]Enter how many drinks to view per page: [/]");
        }

        Console.Clear();
        AnsiConsole.Status()
            .Start("Retrieving drinks...", ctx =>
            {
                Thread.Sleep(2000);
            });

        var (isDisplayed, drinkId) = UiHelpers
            .ShowPaginatedItems(drinks, "drinks", ViewHelpers.DisplayDrinkSummaries,
            pageSize, true, "the id for the drink you wish to view (0 to cancel) ");

        if (!isDisplayed || string.IsNullOrWhiteSpace(drinkId)) return string.Empty;

        return drinkId;
    }

    private async Task ViewDrinkDetails(string drinkId)
    {
        await AnsiConsole.Status()
            .StartAsync("Retrieving drink details...", async ctx =>
            {
                await Task.Delay(2000);
            });

        var drink = await _drinkService.GetDrinkDetailAsync(drinkId);

        if (drink is null)
        {
            UiHelpers
                .PressAnyKeyToContinueError($"Unable to display drink info for id {drinkId}");
            return;
        }

        var drinkDetail = drink.ToDrinkDetailDto();

        drinkDetail.IngredientDetails = await GetIngredientDetailsAsync(drink);

        var table = new Table()
            .BorderColor(Color.DarkMagenta);
        table.AddColumn("[Gold1]Drink Name[/]");
        table.AddColumn("[Gold1]Category[/]");
        table.AddColumn("[Gold1]Alcoholic Content[/]");
        table.AddColumn("[Gold1]Instructions[/]");

        table.AddRow(
            $"[DarkSeaGreen2]{drinkDetail.DrinkName}[/]", 
            $"[DarkSeaGreen2]{drinkDetail.Category}[/]", 
            $"[DarkSeaGreen2]{drinkDetail.Alcoholic}[/]", 
            $"[DarkSeaGreen2]{drinkDetail.Instructions}[/]");

        AnsiConsole.Write(table);

        if (drinkDetail.IngredientMeasures.Count > 0)
        {
            table = new Table()
                .BorderColor(Color.DarkMagenta);
            table.AddColumn("[Gold1]Measurements of the ingredients needed to make this drink[/]");

            foreach (var ingredientMeasure in drinkDetail.IngredientMeasures)
            {
                table.AddRow($"[DarkSeaGreen2]{ingredientMeasure}[/]");
            }

            AnsiConsole.Write(table);

            if (drinkDetail.IngredientDetails.Count > 0)
            {
                var wishesToSeeIngredientDetails = AnsiConsole.Confirm("[Khaki3]Would you like to see the details of the ingredients of this drink? [/]");
                if (wishesToSeeIngredientDetails)
                {
                    UiHelpers
                        .ShowPaginatedItems(drinkDetail.IngredientDetails,
                        $"details of the ingredients in {drinkDetail.DrinkName}", ViewHelpers.DisplayIngredientDetails);
                }
            }
            Console.Clear();
        }
    }
    private static DrinkSearchOptions GetDrinkSearchChoice()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<DrinkSearchOptions>()
            .Title("[CadetBlue]Please choose how you would like to search for drinks[/]")
            .AddChoices(Enum.GetValues<DrinkSearchOptions>())
            .UseConverter(choice => choice.GetDisplayName()));
    }

    private async Task<string> GetCategoryNameAsync()
    {
        await AnsiConsole.Status()
            .StartAsync("Retrieving categories...", async ctx =>
            {
                await Task.Delay(2000);
            });

        var categories = await _categoryService.GetCategoriesAsync();
        
        var categoryChoice = AnsiConsole.Prompt(
            new SelectionPrompt<Category>()
            .Title("[CadetBlue]Choose one of the following categories to search for drinks on[/]")
            .AddChoices(categories)
            .UseConverter(choice => choice.CategoryName));

        return categoryChoice.CategoryName;
    }

    private async Task<string> GetIngredientNameAsync()
    {
        await AnsiConsole.Status()
           .StartAsync("Retrieving ingredients...", async ctx =>
           {
               await Task.Delay(2000);
           });
        
        var ingredients = await _ingredientService.GetIngredientsAsync();

        var ingredientChoice = AnsiConsole.Prompt(
            new SelectionPrompt<Ingredient>()
            .Title("[CadetBlue]Choose one of the following ingredients to search for drinks on[/]")
            .AddChoices(ingredients)
            .UseConverter(choice => choice.IngredientName));

        return ingredientChoice.IngredientName;
    }

    private async Task<string> GetAlcoholicNameAsync()
    {
        await AnsiConsole.Status()
            .StartAsync("Retrieving alcoholic options...", async ctx =>
            {
                await Task.Delay(2000);
            });

        var alcoholics = await _alcoholicService.GetAlcoholicsAsync();
        
        var alcoholicChoice = AnsiConsole.Prompt(
            new SelectionPrompt<Alcoholic>()
            .Title("[CadetBlue]Choose one of the following alcoholic options to search for drinks on[/]")
            .AddChoices(alcoholics)
            .UseConverter(choice => choice.AlcoholicName));

        return alcoholicChoice.AlcoholicName;
    }

    private async Task<string> GetGlassNameAsync()
    {
        await AnsiConsole.Status()
            .StartAsync("Retrieving glass options...", async ctx =>
            {
                await Task.Delay(2000);
            });

        var glasses = await _glassService.GetGlassesAsync();

        var glassChoice = AnsiConsole.Prompt(
            new SelectionPrompt<Glass>()
            .Title("[CadetBlue]Choose one of the following glass options to search for drinks on[/]")
            .AddChoices(glasses)
            .UseConverter(choice => choice.GlassName));

        return glassChoice.GlassName;
    }

    private async Task<List<IngredientDetailDto>> GetIngredientDetailsAsync(DrinkDetail drink)
    {
        var ingredientDetails = new List<IngredientDetailDto>();

        var ingredientNames = drink.GetDrinkProperties("Ingredient");

        foreach (var ingredientName in ingredientNames)
        {
            var ingredientDetail = await _ingredientDetailService.GetIngredientDetailAsync(ingredientName);

            if (ingredientDetail is not null)
            {
                ingredientDetails.Add(ingredientDetail.ToIngredientDetailDto());
            }
        }

        foreach (var ingredient in ingredientDetails)
        {
            if (ingredient is not null && ingredient.Description is not null)
            {
                if (ingredient.Description.Contains("["))
                {
                    ingredient.Description = ingredient.Description.Replace("[", "");
                }
                if (ingredient.Description.Contains("]"))
                {
                    ingredient.Description = ingredient.Description.Replace("]", "");
                }
            }
        }

        return ingredientDetails;
    }

    private List<DrinkSummaryDto> GetDrinkSummaries(List<DrinkSummary> drinks)
    {
        return drinks
            .Select(d => d.ToDrinkSummaryDto())
            .ToList();
    }

    private async Task SearchDrinksAsync(DrinkSearchOptions option, string search)
    {
        var drinks = new List<DrinkSummary>();
        var drinkSummaries = new List<DrinkSummaryDto>();
        var drinkId = string.Empty;

        drinks = await _drinkService.GetDrinksAsync(option, search);
        drinkSummaries = GetDrinkSummaries(drinks);
        drinkId = ViewDrinksAndGetDrinkId(drinkSummaries);

        if (drinkId == "0")
        {
            UiHelpers
                .PressAnyKeyToContinue("Returning to the previous menu");
            return;
        }

        await ViewDrinkDetails(drinkId);
    }
}
