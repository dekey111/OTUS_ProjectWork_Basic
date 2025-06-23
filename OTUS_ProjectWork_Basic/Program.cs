using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Interfaces;
using OTUS_ProjectWork_Basic.Repositories;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

Console.WriteLine("Hello, World!");

// Создание конфигурации
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Настройка сервисов
var services = new ServiceCollection();

// Добавление DbContext
services.AddDbContext<CloudReaderContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

// Регистрация репозиториев
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddScoped<IBookRepository, BookRepository>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();

// Регистрация сервиса бота
services.AddSingleton<TelegramBotService>(provider =>
    new TelegramBotService(
        provider.GetRequiredService<IBookRepository>(),
        provider.GetRequiredService<ICartRepository>(),
        provider.GetRequiredService<IOrderRepository>(),
        configuration["BotToken"]));

// Создание провайдера сервисов
var serviceProvider = services.BuildServiceProvider();

// Запуск бота
try
{
    var botService = serviceProvider.GetRequiredService<TelegramBotService>();
    await botService.StartAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}