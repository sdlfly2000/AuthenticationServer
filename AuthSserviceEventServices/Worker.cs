using EasyNetQ;
using Infra.Core.MessageQueue;

namespace AuthServiceEventServices
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MessageQueueRegister _register;
        private readonly IBus _eventBus;

        public Worker(ILogger<Worker> logger, MessageQueueRegister register)
        {
            _logger = logger;
            _register = register;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _register.Register("AuthServiceEventServices");

            return Task.CompletedTask;
        }
    }
}
