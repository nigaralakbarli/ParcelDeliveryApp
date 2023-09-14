﻿namespace OrderMicroservice.Services.Abstraction;

public interface IKafkaService
{
    void Produce(string topic, string message);
    void Consume(string topic);
}
