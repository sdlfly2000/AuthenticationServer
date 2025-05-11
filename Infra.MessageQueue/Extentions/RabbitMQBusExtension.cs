using EasyNetQ;
using Infra.MessageQueue.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MessageQueue.Extentions
{
    public static class RabbitMqBusExtension
    {
        public static IServiceCollection AddRabbitMQBus(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

            services.AddScoped<IAdvancedBus>(svc => RabbitHutch.CreateBus(
                rabbitMqConfig.Host,
                rabbitMqConfig.Port,
                rabbitMqConfig.VirtualHost,
                rabbitMqConfig.UserName,
                rabbitMqConfig.Password,
                TimeSpan.FromSeconds(30),
                s => s.EnableSystemTextJson()).Advanced);

            return services;
        }
    }
}
