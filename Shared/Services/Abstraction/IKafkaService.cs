﻿namespace Shared.Services.Abstraction;

public interface IKafkaService
{
    void PublishMessage(string topic, string key, string value);
    void ConsumeMessages(string topic, Action<string> messageHandler);
}
