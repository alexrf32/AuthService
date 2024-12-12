using RabbitMQ.Client;
using System.Text;

public class RabbitMqService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void SendMessage(string message)
    {
        
        _channel.QueueDeclare(queue: "hello", 
                              durable: false, 
                              exclusive: false,
                              autoDelete: false, 
                              arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "", 
                              routingKey: "hello", 
                              basicProperties: null, 
                              body: body);
        Console.WriteLine($" [x] Sent {message}");

        _channel.Close();
        _connection.Close();
    }
}

