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
        private const string _directExchange = "direct-exchange";
        private const string _topicExchange = "topic-exchange";
        private const string _fanoutExchange = "fanout-exchange";

        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare(exchange: _directExchange,
                                    type: ExchangeType.Direct,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);

            channel.ExchangeDeclare(exchange: _topicExchange,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);

            channel.ExchangeDeclare(exchange: _fanoutExchange,
                                    type: ExchangeType.Fanout,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);
            var count = 1;
            var list = new List<string>
            {
                ExchangeType.Direct,
                ExchangeType.Topic,
                ExchangeType.Fanout
            };
            while (true)
            {
                var type = list.OrderBy(x => Guid.NewGuid()).First();
                var message = new
                {
                    Name = "Producer",
                    Message = $"Hello! Count {count}"
                };
                string bodyString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(bodyString);

                switch (type)
                {
                    case ExchangeType.Direct:
                        channel.BasicPublish(_directExchange, type, null, body);
                        Console.WriteLine($"Exchange: \"{_directExchange}\" RoutingKey: \"{type}\" Body: {bodyString}");
                        break;

                    case ExchangeType.Topic:
                        channel.BasicPublish(_topicExchange, $"{type}.2", null, body);
                        Console.WriteLine($"Exchange: \"{_topicExchange}\" RoutingKey: \"{type}.2\" Body: {bodyString}");
                        break;

                    case ExchangeType.Fanout:
                        channel.BasicPublish(_fanoutExchange, type, null, body);
                        Console.WriteLine($"Exchange: \"{_fanoutExchange}\" RoutingKey: \"{type}\" Body: {bodyString}");
                        break;

                    default:
                        break;
                }

                count++;
                Thread.Sleep(Random.Shared.Next(200, 1000));
            }
        }
    }
}
