using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Book.ConsoleApp;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\CsharpProjects\Assignment\EFCore\Infrastructure\Data\Local_Database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<BookPublisherRepository>();
    services.AddScoped<BookRepository>();
    services.AddScoped<CategoryRepository>();
    services.AddScoped<DescriptionRepository>();
    services.AddScoped<ManufactureRepository>();

    services.AddScoped<BookService>();
    services.AddScoped<CategoryService>();
    services.AddScoped<DescriptionService>();
    services.AddScoped<ManufactureService>();


    services.AddSingleton<ConsoleAppUI>();

}).Build();

Console.Clear();
var consoleUi = builder.Services.GetRequiredService<ConsoleAppUI>();
await consoleUi.Show();
