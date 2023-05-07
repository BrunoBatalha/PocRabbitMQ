using System.Text;
using RabbitMQ.Client;


var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 5672
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string QUEUEU_NAME = "hello";

channel.QueueDeclare(
    queue: QUEUEU_NAME,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null
);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

for (int i = 0; i < 10; i++)
{
    string message = "Hello, Rabbit MQ "+ i;
    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: QUEUEU_NAME,
        basicProperties: null,
        body: body
    );
}

Console.WriteLine("Messages sent");
