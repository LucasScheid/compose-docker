namespace api_read.service.Interfaces
{
    public interface IReceive
    {
        string ReceiveMessage(string rabbitHost, string queueName);

    }
}
