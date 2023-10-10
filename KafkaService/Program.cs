using Shared.Services.Abstraction;
using Shared.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IKafkaService, KafkaService>(provider =>
{
    return new KafkaService("kafka:9092");
});

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
{


    var kafka = scope.ServiceProvider.GetRequiredService<IKafkaService>();

    var order = scope.ServiceProvider.GetRequiredService<IKafkaService>();


    // Delivery Microservice
    kafka.CreateTopicAsync("order-delivered", 5, 1);
    kafka.CreateTopicAsync("order-assigned", 5, 1);

    // Order Microservice
    kafka.CreateTopicAsync("order-created", 5, 1);
    kafka.CreateTopicAsync("order-updated", 5, 1);
    kafka.CreateTopicAsync("order-deleted", 5, 1);
    kafka.CreateTopicAsync("order-status", 5, 1);
    kafka.CreateTopicAsync("order-destination", 5, 1);
    kafka.CreateTopicAsync("order-canceled", 5, 1);

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
