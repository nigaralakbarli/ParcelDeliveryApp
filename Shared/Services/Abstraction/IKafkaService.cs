namespace Shared.Services.Abstraction;

public interface IKafkaService
{
    void PublishMessage(string topic, string key, string value);
    void ConsumeMessages(Dictionary<string, Action<string>> topicHandlers);
    Task CreateTopicAsync(string topicName, int numPartitions, short replicationFactor);
}
