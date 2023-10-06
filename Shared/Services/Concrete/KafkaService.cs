using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Shared.Services.Abstraction;
using System.Text;

namespace Shared.Services.Concrete;

public class KafkaService : IKafkaService
{
    private readonly IProducer<string, string> producer;
    private readonly IConsumer<string, string> consumer;
    private readonly AdminClientConfig adminClientConfig;

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

        consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

        adminClientConfig = new AdminClientConfig
        {
            BootstrapServers = bootstrapServers,
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

    public async Task CreateTopicAsync(string topicName, int numPartitions, short replicationFactor)
    {
        using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification>
            {
                new TopicSpecification
                {
                    Name = topicName,
                    NumPartitions = numPartitions,
                    ReplicationFactor = replicationFactor
                }
            });
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred creating the topic {topicName}: {e.Message}");
            }
        }
    }
}