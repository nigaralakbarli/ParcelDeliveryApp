using Confluent.Kafka;
using DeliveryMicroservice.Services.Abstraction;

namespace DeliveryMicroservice.Services.Concrete;

public class KafkaService : IKafkaService
{
    private readonly IProducer<string, string> _producer;
    private readonly IConsumer<string, string> _consumer;

    public KafkaService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "your-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

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
