using Spectre.Console;

namespace DrinksInfo.TerrenceLGee.DrinksUi.Helpers;

public static class UiHelpers
{
    public static (bool, string?) ShowPaginatedItems<T>(
        List<T> items,
        string name,
        Action<List<T>> display,
        int pageSize = 10,
        bool returnId = false,
        string idFor = "")
    {
        if (items.Count == 0)
        {
            PressAnyKeyToContinueError($"There currently are no {name} available");
            return (false, null);
        }

        string? id = null;
        var pageIndex = 0;
        var pageCount = (int)Math.Ceiling(items.Count / (double)pageSize);

        while (true)
        {
            var pagedItems = items
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            AnsiConsole.MarkupLine($"[DarkOliveGreen1] " +
                $"Page {pageIndex + 1} of {pageCount} (showing {pagedItems.Count} of {items.Count})[/]");

            display(pagedItems);

            var prompt = new SelectionPrompt<Choice>()
                .Title("[DarkOrange]Navigate pages: [/]");

            if (pageIndex > 0)
            {
                prompt.AddChoice(Choice.Previous);
            }

            if (pageIndex < pageCount - 1)
            {
                prompt.AddChoice(Choice.Next);
            }

            if (returnId)
            {
                prompt.AddChoice(Choice.Choose);
            }
            else
            {
                prompt.AddChoice(Choice.Exit);
            }

            var choice = AnsiConsole.Prompt(prompt);

            if (choice == Choice.Next && pageIndex < pageCount - 1)
            {
                pageIndex++;
                Console.Clear();
            }
            else if (choice == Choice.Previous && pageIndex > 0)
            {
                pageIndex--;
                Console.Clear();
            }
            else
            {
                break;
            }
        }
        if (returnId)
        {
            id = AnsiConsole.Ask<string>($"[DarkSeaGreen1]\nEnter {idFor}: [/]");
        }

        PressAnyKeyToContinue();
        return (true, id);
    }

    public static void PressAnyKeyToContinue(string message = "")
    {
        AnsiConsole.MarkupLine($"[Gold1]{message}[/]");
        AnsiConsole.MarkupLine("[DarkTurquoise]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    public static void PressAnyKeyToContinueError(string errorMessage)
    {
        AnsiConsole.MarkupLine($"[bold underline red]{errorMessage}[/]");
        AnsiConsole.MarkupLine("[DarkTurquoise]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
}

public enum Choice
{
    Previous,
    Next,
    Choose,
    Exit
}
