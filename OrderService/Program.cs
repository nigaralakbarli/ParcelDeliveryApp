using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderMicroservice.DbContext;
using OrderMicroservice.Repositories.Abstraction;
using OrderMicroservice.Repositories.Concrete;
using OrderMicroservice.Services.Abstraction;
using OrderMicroservice.Services.Concrete;
using Serilog;
using Shared.Models;
using Shared.Services.Abstraction;
using Shared.Services.Concrete;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User Management",
        Description = "ASP.NET Core Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["TokenOption:Issuer"],
                ValidAudience = builder.Configuration["TokenOption:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOption:Key"]))
            };
        });

builder.Services.AddDbContext<OrderDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddSingleton<IKafkaService, KafkaService>(provider =>
{
    return new KafkaService("kafka:9092");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});

builder.Host.UseSerilog();

var app = builder.Build();


// Configure the HTTP request pipeline.

await using var scope = app.Services.CreateAsyncScope();
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    await db.Database.MigrateAsync();

    var kafka = scope.ServiceProvider.GetRequiredService<IKafkaService>();

    var order = scope.ServiceProvider.GetRequiredService<IOrdersService>();


    Task.Run(() => kafka.ConsumeMessages("order-status", order.OrderDeliveredEventHandler));

    Task.Run(() => kafka.ConsumeMessages("order-assigned", order.OrderAssignedEventHandler));
}

app.UseSwagger();

app.UseSerilogRequestLogging();

app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
