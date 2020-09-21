using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using send.message.rabbit_api.Model;
using System.Text;

namespace send.message.rabbit_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly RabbitConfig _rabbitConfig;


        public MessageController(RabbitConfig rabbitConfig)
        {
            _rabbitConfig = rabbitConfig;
        }


        [HttpPost]
        public IActionResult Send(string message)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitConfig.Host

                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                {
                    channel.QueueDeclare(queue: _rabbitConfig.QueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: _rabbitConfig.QueueName,
                                         basicProperties: properties,
                                         body: body);
                }

                channel.Close();
                connection.Close();

                return Ok($"Send {message} to queue => {_rabbitConfig.QueueName}");

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
