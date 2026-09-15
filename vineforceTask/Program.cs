using Microsoft.EntityFrameworkCore;
using vineforceTask.DatabaseConnect;
using vineforceTask.Repo.Implementation;
using vineforceTask.Repo.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers
builder.Services.AddControllers();

// Entity Framework Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions =>
        {
            sqlServerOptions.UseCompatibilityLevel(120);
        }
    )
);

// Repository Dependency Injection
builder.Services.AddScoped<ICountryRepository, CountryImplementation>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - Allow Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();