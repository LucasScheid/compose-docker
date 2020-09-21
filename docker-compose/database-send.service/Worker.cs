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
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                var receiveMessage = _receive.ReceiveMessage(rabbitHost, queueResultName);

                //Root apiResult = new Root();

                List<ApiResult> json = JsonConvert.DeserializeObject<List<ApiResult>>(receiveMessage);

                if (json.Count > 0)
                {

                    try
                    {
                        using (var conn = new NpgsqlConnection(conString))

                        {
                            Console.Out.WriteLine("Opening connection");
                            conn.Open();

                            foreach (var item in json)
                            {
                                using (var command = new NpgsqlCommand("INSERT INTO covid_result.countrys (Country,CountryCode,Province,City,CityCode,Lat,Lon,Confirmed,Deaths,Recovered,Active,Date) VALUES (@Country,@CountryCode,@Province,@City,@CityCode,@Lat,@Lon,@Confirmed,@Deaths,@Recovered,@Active,@Date)", conn))
                                {
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


                                    int nRows = command.ExecuteNonQuery();
                                    //Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                                }
                            }


                            //using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS inventory", conn))
                            //{
                            //    command.ExecuteNonQuery();
                            //    Console.Out.WriteLine("Finished dropping table (if existed)");

                            //}

                            //using (var command = new NpgsqlCommand("CREATE TABLE inventory(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                            //{
                            //    command.ExecuteNonQuery();
                            //    Console.Out.WriteLine("Finished creating table");
                            //}

                            //using (var command = new NpgsqlCommand("INSERT INTO inventory (name, quantity) VALUES (@n1, @q1), (@n2, @q2), (@n3, @q3)", conn))
                            //{
                            //    command.Parameters.AddWithValue("n1", "banana");
                            //    command.Parameters.AddWithValue("q1", 150);
                            //    command.Parameters.AddWithValue("n2", "orange");
                            //    command.Parameters.AddWithValue("q2", 154);
                            //    command.Parameters.AddWithValue("n3", "apple");
                            //    command.Parameters.AddWithValue("q3", 100);

                            //    int nRows = command.ExecuteNonQuery();
                            //    Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                            //}
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message); ;
                    }

                }








































                await Task.Delay(delayTime, stoppingToken);
            }
        }
    }
}
