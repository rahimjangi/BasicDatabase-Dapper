using BasicDatabase;
using BasicDatabase.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Set up the dependency injection
var serviceProvider = ConfigureServices();

// Get the repository from the service provider
var computerRepository = serviceProvider.GetService<IComputerRepository>();

// Example CRUD operations
string identifier = "ComputerIdentifier";

// Create a new computer
var newComputer = new Computer
{
    Motherboard = "ASUS ROG",
    CPUcores = 8,
    HasWiFi = true,
    HasLTE = false,
    ReleaseDate = DateTime.Now,
    Price = 999.99m,
    VideoCard = "NVIDIA RTX 3080"
};

int newId = computerRepository.CreateComputer(newComputer, identifier);
Console.WriteLine($"New computer created with ID: {newId}");

// Get the computer by ID
var computer = computerRepository.GetComputerById(newId, identifier);
Console.WriteLine($"Retrieved computer: {computer.Motherboard}, {computer.Price}");

// Update the computer
computer.Price = 899.99m;
computerRepository.UpdateComputer(computer, identifier);
Console.WriteLine("Computer updated.");

// Delete the computer
computerRepository.DeleteComputer(newId, identifier);
Console.WriteLine("Computer deleted.");


static IServiceProvider ConfigureServices()
{
    // Build configuration
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    // Set up dependency injection
    var services = new ServiceCollection();
    services.AddSingleton<IConfiguration>(configuration);

    // Configure logging
    services.AddLogging(configure => configure.AddConsole());

    // Register the repository
    services.AddScoped<IComputerRepository>(provider =>
        new ComputerRepository(
            configuration.GetConnectionString("DefaultConnection"),
            provider.GetRequiredService<ILogger<ComputerRepository>>()
        )
    );

    return services.BuildServiceProvider();
}





