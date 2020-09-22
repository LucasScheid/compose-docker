using api_read.service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace api_read.service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReceive _receive;
        private readonly ISend _send;
        private readonly ICovidApiRequest _covidApiRequest;

        public Worker(ILogger<Worker> logger, IReceive receive, ISend send, ICovidApiRequest covidApiRequest)
        {
            _logger = logger;
            _receive = receive;
            _send = send;
            _covidApiRequest = covidApiRequest;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueEntryName = Environment.GetEnvironmentVariable("RABBIT_MQ_ENTRY_QUEUE");
            var queueResultName = Environment.GetEnvironmentVariable("RABBIT_MQ_RESULT_QUEUE");
            var rabbitHost = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST");
            var delayTime = Convert.ToInt32(Environment.GetEnvironmentVariable("DELAY_TIME"));
            var baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            var ServiceURL = Environment.GetEnvironmentVariable("SERVICE_URL");

            while (!stoppingToken.IsCancellationRequested)
            {
                var finalMessage = _receive.ReceiveMessage(rabbitHost, queueEntryName);

                if (!string.IsNullOrEmpty(finalMessage))
                {
                    Console.WriteLine("Iniciar consulta....");

                    var apiResult = await _covidApiRequest.Execute(baseURL, ServiceURL, finalMessage);

                    if (!string.IsNullOrEmpty(apiResult))
                    {
                        Console.WriteLine("Consultou....");
                        _send.SendMessage(apiResult, rabbitHost, queueResultName);
                    }
                }

                await Task.Delay(delayTime, stoppingToken);
            }
        }
    }
}
