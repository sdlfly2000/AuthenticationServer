using EasyNetQ;
using MessageQueue.RabbitMQ.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueue.RabbitMQ.Extentions
{
    public static class RabbitMqBusExtension
    {
        public static IServiceCollection AddRabbitMQBus(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

            services.AddSingleton(svc => RabbitHutch.CreateBus(
                rabbitMqConfig.Host,
                rabbitMqConfig.Port,
                rabbitMqConfig.VirtualHost,
                rabbitMqConfig.UserName,
                rabbitMqConfig.Password,
                TimeSpan.FromSeconds(30),
                s => s.EnableSystemTextJson()));

            return services;
        }
    }
}
