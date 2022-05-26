using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer
{
    public class QueueProducer
    {
        public static void Publish(IModel channel)
        {
            channel.QueueDeclare(queue: "demo-queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: true,
                                             arguments: null);
            var count = 0;
            while (true)
            {
                var message = new
                {
                    Name = "Producer",
                    Message = $"Hello! Count {count}"
                };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("", "demo-queue", null, body);
                count++;
                Thread.Sleep(Random.Shared.Next(500, 2000));
            }
        }
    }
}
