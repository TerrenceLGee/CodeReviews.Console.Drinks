using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.DrinksUi;
using DrinksInfo.TerrenceLGee.DrinksUi.Interfaces;
using DrinksInfo.TerrenceLGee.Services;
using DrinksInfo.TerrenceLGee.Services.FilterServices;
using DrinksInfo.TerrenceLGee.Services.Interfaces.DrinkServiceInterfaces;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using DrinksInfo.TerrenceLGee.Services.Interfaces.IngredientServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

try
{
    LoggingSetup();
    await Startup();
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}


return;

async Task Startup()
{
    var services = new ServiceCollection()
        .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
        .AddScoped<ICategoryService, CategoryService>()
        .AddScoped<IAlcoholicService, AlcoholicService>()
        .AddScoped<IGlassService, GlassService>()
        .AddScoped<IIngredientService, IngredientService>()
        .AddScoped<IDrinkService, DrinkService>()
        .AddScoped<IIngredientDetailService, IngredientDetailService>()
        .AddScoped<IDrinksUi, DrinksUi>();

    services.AddHttpClient("client", c =>
    {
        c.BaseAddress = new Uri(Urls.BaseUrl);
    });

    var serviceProvider = services.BuildServiceProvider();

    var drinksUi = serviceProvider.GetRequiredService<IDrinksUi>();

    var app = new DrinksInfoApp(drinksUi);

    await app.Run();
}

void LoggingSetup()
{
    var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(loggingDirectory);
    var filePath = Path.Combine(loggingDirectory, "app-.txt");
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Error()
        .WriteTo.File(
        path: filePath,
        rollingInterval: RollingInterval.Day,
        outputTemplate: outputTemplate)
        .CreateLogger();
}
