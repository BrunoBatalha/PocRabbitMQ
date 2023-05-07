using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += async (mode,ea) =>{
    Console.WriteLine("Processando message: " + mode);
    await Task.Delay(1000);
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(
    queue: QUEUEU_NAME,
    autoAck: false,
    consumer: consumer
);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();