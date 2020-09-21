namespace api_read.service.Interfaces
{
    public interface ISend
    {
        void SendMessage(string messageBody, string rabbitHost, string queueName);
    }
}
