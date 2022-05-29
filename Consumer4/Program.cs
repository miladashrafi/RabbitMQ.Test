
using RabbitMQ.Client;

namespace Consumer4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672"),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromDays(1),
                TopologyRecoveryEnabled = true
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            QueueConsumer.Consume(channel);
        }
    }
}