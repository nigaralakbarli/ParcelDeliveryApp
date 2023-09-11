using Confluent.Kafka;
using OrderService.Services.Abstraction;

namespace OrderService.Services.Concrete;

public class KafkaService : IKafkaService
{
    private readonly IProducer<string, string> _producer;
    private readonly IConsumer<string, string> _consumer;

    public KafkaService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            // Other producer configuration options
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "your-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            // Other consumer configuration options
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }
    //implement kafka topic here 
    public void Produce(string topic, string message)
    {
        _producer.Produce(topic, new Message<string, string> { Value = message });
        
    }

    public void Consume(string topic)
    {
        _consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = _consumer.Consume();
            Console.WriteLine($"Received message: {consumeResult.Message.Value}");
        }
    }
}