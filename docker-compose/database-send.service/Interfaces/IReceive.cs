namespace database_send.service.Interfaces
{
    public interface IReceive
    {
        string ReceiveMessage(string rabbitHost, string queueName);

    }
}
