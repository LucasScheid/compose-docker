using api_read.service.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace api_read.service.Services
{
    public class Send : ISend
    {
        public void SendMessage(string messageBody, string rabbitHost, string queueName)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitHost
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(messageBody);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: properties,
                                     body: body);

                channel.Close();
                connection.Close();

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"SendMessage Exception ===> {ex.Message}");
            }

        }
    }
}
