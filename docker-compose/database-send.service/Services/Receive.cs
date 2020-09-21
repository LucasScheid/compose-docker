using database_send.service.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace database_send.service.Services
{
    public class Receive : IReceive
    {

        public string ReceiveMessage(string rabbitHost, string queueName)
        {
            try
            {
                string finalMessage = string.Empty;

                var factory = new ConnectionFactory()
                {
                    HostName = rabbitHost,
                    DispatchConsumersAsync = true
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        finalMessage = message;
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        await Task.Yield();
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Thread.Sleep(7000);
                }

                channel.Close();
                connection.Close();

                //System.Console.WriteLine($"ReceiveMessage ===> {finalMessage}");
                return finalMessage;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ReceiveMessage Exception ===> {ex.Message}");
            }

            return string.Empty;
        }
    }
}
