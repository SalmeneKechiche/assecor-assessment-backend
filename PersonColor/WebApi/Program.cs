using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var env = builder.Environment;

bool useDatabase = config.GetValue<bool>("UseDatabase");

var csvPath = Path.Combine(AppContext.BaseDirectory, builder.Configuration.GetValue<string>("CsvFilePath")!);

if (!File.Exists(csvPath))
    throw new FileNotFoundException($"CSV file not found at {csvPath}");

// Services registrieren
if (useDatabase)
{
    // Ensure the Data directory exists for SQLite database
    var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
    if (!Directory.Exists(dataDirectory))
    {
        Directory.CreateDirectory(dataDirectory);
    }

    builder.Services.AddDbContext<PersonDbContext>(options =>
        options.UseSqlite(config.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IPersonRepository, DatabasePersonRepository>();
}
else
{
    if (!File.Exists(csvPath))
        throw new FileNotFoundException($"CSV file not found at {csvPath}");

    builder.Services.AddSingleton<IPersonRepository>(new CsvPersonRepository(csvPath));
}

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PersonColor API", Version = "v1" });

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Datenbank initialisieren
if (useDatabase)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PersonDbContext>();

    await context.Database.MigrateAsync();

    if (!context.Persons.Any())
    {
        var csvRepository = new CsvPersonRepository(csvPath);
        var persons = await csvRepository.GetAllAsync();

        context.Persons.AddRange(persons
            .Where(p => !context.Persons.Any(x => x.Id == p.Id)));

        await context.SaveChangesAsync();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonColor API V1");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
