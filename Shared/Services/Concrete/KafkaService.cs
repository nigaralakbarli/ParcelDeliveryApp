using Confluent.Kafka;
using Shared.Services.Abstraction;
using System.Text;

namespace Shared.Services.Concrete;

public class KafkaService : IKafkaService
{
    private readonly IProducer<string, string> producer;
    private readonly IConsumer<string, string> consumer;

    public KafkaService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            ClientId = "kafka-producer-client"
        };

        producer = new ProducerBuilder<string, string>(producerConfig).Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "kafka-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

    }

    public void PublishMessage(string topic, string key, string value)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = value
        };

        producer.Produce(topic, message);
    }

    public void ConsumeMessages(string topic, Action<string> messageHandler)
    {
        consumer.Subscribe(topic);

        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume();
                messageHandler(consumeResult.Message.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling Kafka message: {ex.Message}");
            }
        }
    }
}