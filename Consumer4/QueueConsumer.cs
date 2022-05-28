﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Consumer4
{
    public class QueueConsumer
    {
        private const string _directExchange = "direct-exchange";
        private const string _topicExchange = "topic-exchange";
        private const string _fanoutExchange = "fanout-exchange";

        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(exchange: _directExchange,
                                    type: ExchangeType.Direct,
                                    durable: true,
                                    autoDelete: true,
                                    arguments: null);

            channel.ExchangeDeclare(exchange: _topicExchange,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: true,
                                    arguments: null);

            channel.ExchangeDeclare(exchange: _fanoutExchange,
                                    type: ExchangeType.Fanout,
                                    durable: true,
                                    autoDelete: true,
                                    arguments: null);

            var queueName = "fanout-queue2";
            channel.QueueDeclare(queueName, true, false, true, null);

            //var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                              exchange: _fanoutExchange,
                              routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queueName, false, consumer);
            Console.ReadLine();
        }

        private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"Exchange: \"{e.Exchange}\" RoutingKey: \"{e.RoutingKey}\" Body: {body}");
            ((EventingBasicConsumer?)sender)?.Model?.BasicAck(e.DeliveryTag, false);
        }
    }
}
