using api_read.service.Interfaces;
using api_read.service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api_read.service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ICovidApiRequest, CovidApiRequest>();
                    services.AddTransient<IReceive, Receive>();
                    services.AddTransient<ISend, Send>();

                    services.AddHostedService<Worker>();
                });
    }
}
