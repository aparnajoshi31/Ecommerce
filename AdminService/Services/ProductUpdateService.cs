using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using AdminService.Models;
using System;

public class ProductUpdateService
{
    private readonly IModel _channel;

    public ProductUpdateService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "product_updates",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublishProductDetailUpdate(ProductModel detail)
    {
        try
        {
            string message = JsonSerializer.Serialize(detail);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: "product_updates",
                                 basicProperties: null,
                                 body: body);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to publish product detail update: {ex.Message}");
        }
    }
}