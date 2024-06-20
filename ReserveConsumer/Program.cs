using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services;
using System.Data;
using System.Text;

namespace ReserveConsumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string QUEUE_NAME = "salesReservation";

            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();


            var service = new SaleService();


            channel.QueueDeclare(
                queue: QUEUE_NAME,
                durable: false,
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

                    var message = Encoding.UTF8.GetString(body);

                    var sale = JsonConvert.DeserializeObject<SaleDTO>(message);

                    sale = service.Post(sale).Result;

                    if (sale == null)
                        Console.WriteLine("Houve um erro ao inserir a venda");
                    else
                        Console.WriteLine("Venda inserida com sucesso");

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(
                        queue: QUEUE_NAME,
                        autoAck: false,
                        consumer: consumer
                    );

                Thread.Sleep(2000);
            }
        }
    }
}
