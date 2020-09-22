using database_send.service.Interfaces;
using database_send.service.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace database_send.service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReceive _receive;

        public Worker(ILogger<Worker> logger, IReceive receive)
        {
            _logger = logger;
            _receive = receive;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueResultName = Environment.GetEnvironmentVariable("RABBIT_MQ_RESULT_QUEUE");
            var rabbitHost = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST");
            var delayTime = Convert.ToInt32(Environment.GetEnvironmentVariable("DELAY_TIME"));
            var conString = Environment.GetEnvironmentVariable("POSTGRES_STRING_CONNECTION");

            while (!stoppingToken.IsCancellationRequested)
            {
                string message = string.Empty;
                _logger.LogInformation("Aguardando mensagens... ", DateTimeOffset.Now);

                try
                {
                    message = _receive.ReceiveMessage(rabbitHost, queueResultName);

                    if (!string.IsNullOrEmpty(message))
                    {
                        List<ApiResult> json = JsonConvert.DeserializeObject<List<ApiResult>>(message);

                        if (json.Count > 0)
                        {
                            try
                            {
                                using var conn = new NpgsqlConnection(conString);
                                Console.Out.WriteLine("Opening connection");
                                conn.Open();

                                foreach (var item in json)
                                {
                                    using var command = new NpgsqlCommand("INSERT INTO covid_result.countrys (Country,CountryCode,Province,City,CityCode,Lat,Lon,Confirmed,Deaths,Recovered,Active,Date) VALUES (@Country,@CountryCode,@Province,@City,@CityCode,@Lat,@Lon,@Confirmed,@Deaths,@Recovered,@Active,@Date)", conn);
                                    command.Parameters.AddWithValue("Country", item.Country);
                                    command.Parameters.AddWithValue("CountryCode", item.CountryCode);
                                    command.Parameters.AddWithValue("Province", item.Province);
                                    command.Parameters.AddWithValue("City", item.City);
                                    command.Parameters.AddWithValue("CityCode", item.CityCode);
                                    command.Parameters.AddWithValue("Lat", item.Lat);
                                    command.Parameters.AddWithValue("Lon", item.Lon);
                                    command.Parameters.AddWithValue("Confirmed", item.Confirmed);
                                    command.Parameters.AddWithValue("Deaths", item.Deaths);
                                    command.Parameters.AddWithValue("Recovered", item.Recovered);
                                    command.Parameters.AddWithValue("Active", item.Active);
                                    command.Parameters.AddWithValue("Date", item.Date);

                                    int aff = command.ExecuteNonQuery();
                                    Console.WriteLine($"{aff} rows were affected.");

                                }
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine($"Exception(Bd) ==> { ex.Message}");
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Exception(Bd) ==> { ex.Message}");
                }

                await Task.Delay(delayTime, stoppingToken);
            }
        }
    }
}
