using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using vineforceTask.DatabaseConnect;
using vineforceTask.Repo.Implementation;
using vineforceTask.Repo.Interface;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Controllers + JSON Configuration
// ==========================================
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent Order -> Payment -> Order -> Payment
        // JSON serialization cycle
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// ==========================================
// Database
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions =>
        {
            sqlServerOptions.UseCompatibilityLevel(120);
        }
    )
);

// ==========================================
// HTTP Client Factory
// Required by PaymentImp for Razorpay API
// ==========================================
builder.Services.AddHttpClient();

// ==========================================
// Repository Dependency Injection
// ==========================================
builder.Services.AddScoped<ICountryRepository, CountryImplementation>();
builder.Services.AddScoped<IProducts, ProductIMp>();
builder.Services.AddScoped<IOrder, OrderRepository>();
builder.Services.AddScoped<IPayment, PaymentImp>();

// ==========================================
// Swagger
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// CORS
// ==========================================
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

// ==========================================
// Swagger
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// Middleware
// ==========================================
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();