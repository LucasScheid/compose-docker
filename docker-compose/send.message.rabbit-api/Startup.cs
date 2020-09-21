using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using send.message.rabbit_api.Model;
using System;

namespace send.message.rabbit_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "send.message.rabbit-api", Version = "v1" });
            });

            //var rabbitConfig = new RabbitConfig(Environment.GetEnvironmentVariable("RABBIT_MQ_HOST"), Environment.GetEnvironmentVariable("RABBIT_MQ_ENTRY_QUEUE"));
            services.AddSingleton(new RabbitConfig(Environment.GetEnvironmentVariable("RABBIT_MQ_HOST"), Environment.GetEnvironmentVariable("RABBIT_MQ_ENTRY_QUEUE")));

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(option => option.AllowAnyOrigin());
            app.UseRouting();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "send.message.rabbit-api v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
