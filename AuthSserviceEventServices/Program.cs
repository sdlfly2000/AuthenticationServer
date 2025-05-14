using AuthServiceEventServices;
using Common.Core.DependencyInjection;
using Infra.MessageQueue.Extentions;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSerilog(
    (configure) => configure.ReadFrom.Configuration(builder.Configuration));
//builder.Services.RegisterEasyNetQ();
builder.Services.AddRabbitMQBus(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.RegisterDomain("AuthServiceEventServices", "Infra.MessageQueue", "Infra.Shared.Core", "Infra.Core");

var host = builder.Build();

host.Run();
