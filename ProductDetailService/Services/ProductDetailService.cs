namespace ProductDetailService.Services
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using ProductDetailService.Models;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using System;

    public class ProductDetailProcessingService
    {
        private readonly IModel _channel;
        private readonly List<ProductDetail> _productDetails = new List<ProductDetail>();

        public ProductDetailProcessingService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            Consume();
        }

        public void Consume()
        {
            _channel.QueueDeclare(queue: "product_updates",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var productModel = JsonSerializer.Deserialize<ProductModel>(message);
                if (productModel != null && productModel.Details != null)
                {
                    foreach (var detail in productModel.Details)
                    {
                        if (detail != null)
                        {
                            _productDetails.Add(detail);
                            Console.WriteLine(" [x] Received {0}", detail.Id);
                        }
                    }
                }
            };
            _channel.BasicConsume(queue: "product_updates",
                                 autoAck: true,
                                 consumer: consumer);
        }

        public ProductDetail GetProductDetails(int id)
        {
            var productDetail = _productDetails.Find(p => p.Id == id);
            return productDetail;
        }

        public IEnumerable<ProductDetail> GetAllProductDetails()
        {
            return _productDetails;
        }
    }
}