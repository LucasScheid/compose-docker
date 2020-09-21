namespace send.message.rabbit_api.Model
{
    public class RabbitConfig
    {

        public string Host { get; private set; }
        public string QueueName { get; private set; }


        public RabbitConfig(string host, string queueName)
        {
            Host = host;
            QueueName = queueName;
        }

    }
}
