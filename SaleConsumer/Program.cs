using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Services;

const string QUEUE_NAME = "sold";

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var conn = factory.CreateConnection())
{
    using (var channel = conn.CreateModel())
    {
        channel.QueueDeclare(
            queue: QUEUE_NAME,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        while (true)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var returnQueue = Encoding.UTF8.GetString(body);
                var id = JsonConvert.DeserializeObject<int>(returnQueue);
                new SaleService().SoldSale(id);
                Console.WriteLine(id);
            };

            channel.BasicConsume(
                consumer: consumer,
                queue: QUEUE_NAME,
                autoAck: true
                );

            Thread.Sleep(2000);
        }
    }

}
