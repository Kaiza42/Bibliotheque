using Microsoft.EntityFrameworkCore;
using LibrIO;
using Microsoft.AspNetCore.Components.Forms;
using System.Formats.Tar;
using System;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibrIODb>(opt => opt.UseSqlite("Data Source=LibriIO.db"));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Ajouter DbContext et configuration de la base de donn�es SQLite
builder.Services.AddDbContext<LibrIODb>(options =>
    options.UseSqlite("Data Source=LibrIO.db"));

// Ajouter les services n�cessaires, comme les contr�leurs et Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LibrIO API",
        Version = "v1",
        Description = "Une API pour une biblioth�que"
    });
    c.EnableAnnotations();
});

var app = builder.Build(); // Construire l'application apr�s avoir ajout� les services

// V�rifiez si l'environnement est en d�veloppement pour activer Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibrIO API V1");
        c.RoutePrefix = string.Empty;
    });
}


// Initialiser la base de donn�es avec un json (m�thode seed)
using (var scope = app.Services.CreateScope())
{
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LibrIODb>();
        DbInitializer.Seed(dbContext);
    }
}

app.MapControllers();

// D�marrer l'application
app.Run();

