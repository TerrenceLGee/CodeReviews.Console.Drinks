# Drinks Info App

Console application written with C# 14/.NET 10 using Microsoft's Visual Studio Community 2026 on Windows 11.

This is a console application that retrieves drink information from [TheCocktailDB](https://www.thecocktaildb.com/api.php)

Created following the curriculm of [C# Academy](https://www.thecsharpacademy.com/)

[Drinks Info](https://www.thecsharpacademy.com/project/15/drinks)

## Features
- Calls the API of TheCocktailDB to retrieve information about various cocktails/drinks - alcoholic, non-alcoholic and also optional alcohol.
- Allows the user to filter by category, ingredient, glass served in, drink type (alcoholic etc) as well as by first letter and consecutive characters in the drink's name. Also allows the user to retrieve information about a random drink if they so choose.
- Once the user chooses a drink the application will then display detailed information on the drink (if available) including the ingredients as well as the instructions on how to make the drink.
- Unit tests are included to test the service layer of this application.

## Challenges Faced When Implementing This Project
- Learning how to "model" the JSON data returned from the API. 
- Learning how to simulate calling an external API in unit tests. This was the most challenging aspect and took a few days of googling and reading blogs/stackoverflow posts to begin to understand how to implement this functionality.

## What Was Learned Implementing This Project
- Learned how to extract information from a returned JSON object into C# classes.
- Learned how to call an external API and make use of the data in my own application.
- Learned how to "mock" HttpMessageHandler in order to simulate calling and retrieving information from an external API.
```
       var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoryQuery)
            .ReturnsHttpResponseAsync(CategoryCocktailReponse.GetCategoryCocktailResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);
```

## Areas to improve upon
- Want to learn more about calling external APIs and modeling the data they return.
- Want to learn more about Http status codes and how to interpret what they communicate and how to handle that in an application.
- Want to learn more on how to simulate behavior for unit testing, as well as learn how to make the unit tests that I write cleaner and more robust.

## Technolgies Used
- [Spectre.Console](https://spectreconsole.net/)
- [Serilog](https://serilog.net/)
- [XUnit](https://xunit.net/?tabs=cs)
- [Moq](https://github.com/devlooped/moq)

## Helpful Resources Used
- [Great for mapping JSON to C# classes](https://json2csharp.com/)
- [stackoverflow post on mocking HttpClient](https://stackoverflow.com/questions/62512473/mocking-an-httpclient-created-using-ihttpclientfactory-createclient)
- [The right way to use HttpClient](https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet)
- [How to mock HttpClient in C# using Moq](https://daninacan.com/how-to-mock-httpclient-in-c-using-moq/)
- [Another article about mocking HttpClient](https://medium.com/@niteshsinghal85/mocking-http-request-in-csharp-unit-testing-c4a7cef21828)

### Special credit due to 
[Dan In A Can](https://daninacan.com) 
- For the extension methods from his article on mocking HttpClient using Moq, which I made use of and slightly modified.

### Instructions 
- If you are using Microsoft Visual Studio or JetBrains Rider simply build and run the project.
- If using Visual Studio Code or running this program from the command line run the following commands from the project root directory:
```
dotnet build
dotnet run
```