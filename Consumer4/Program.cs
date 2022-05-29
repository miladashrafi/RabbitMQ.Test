using Consumer4;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .Build();

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@host.docker.internal:5672"),
    AutomaticRecoveryEnabled = true,
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
    TopologyRecoveryEnabled = true,
    RequestedHeartbeat = TimeSpan.FromSeconds(10),
    RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
    SocketReadTimeout = TimeSpan.FromSeconds(10)
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
QueueConsumer.Consume(channel);

await host.RunAsync();