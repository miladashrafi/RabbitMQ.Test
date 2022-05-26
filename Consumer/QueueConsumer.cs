using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Consumer1
{
    public class QueueConsumer
    {
        public static void Consume(IModel channel)
        {
            var queue = channel.QueueDeclare(queue: "demo-queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: true,
                                             arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume("demo-queue", true, consumer);
            Console.ReadLine();
        }

        private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"Consumer1: {body}");
        }
    }
}
