using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Data;
using Order.WebAPI.Helpers;
using Order.WebAPI.Repositories;
using Order.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
    cfg.AllowNullDestinationValues = false;

    cfg.AddProfile<MappingProfile>();
    cfg.AddProfile<GrpcMappingProfile>();

    cfg.AddMaps(typeof(Program).Assembly);
});

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

// gRPC Client
builder.Services.AddSingleton<IInventoryGrpcClient, InventoryGrpcClient>();

// Add HTTP client for gRPC with retry policy
builder.Services.AddGrpcClient<Order.WebAPI.Protos.InventoryService.InventoryServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["InventoryService:Url"] ?? "https://localhost:7001");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

// Seed initial data (optional)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Seed sample data if needed
    await SeedData(context);
}

app.Run();

//-------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------------


// Seed Data Method
static async Task SeedData(ApplicationDbContext context)
{
    if (!context.Customers.Any())
    {
        var customers = new[]
        {
            new Order.WebAPI.Models.Entities.Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@email.com",
                Phone = "123-456-7890",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "10001"
            },
            new Order.WebAPI.Models.Entities.Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@email.com",
                Phone = "123-456-7891",
                Address = "456 Oak Ave",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90210"
            }
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }

    if (!context.Products.Any())
    {
        var products = new[]
        {
            new Order.WebAPI.Models.Entities.Product
            {
                Name = "Sample Product 1",
                Description = "Description for sample product 1",
                SKU = "PROD001",
                Price = 29.99m,
                IsActive = true
            },
            new Order.WebAPI.Models.Entities.Product
            {
                Name = "Sample Product 2",
                Description = "Description for sample product 2",
                SKU = "PROD002",
                Price = 49.99m,
                IsActive = true
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}