using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

public class RabbitMqService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService(string hostName = "localhost")
    {
        var factory = new ConnectionFactory() { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    // Método para enviar mensaje a una cola
    public void SendMessage(string queueName, object message)
    {
        _channel.QueueDeclare(queue: queueName, 
                              durable: true,  
                              exclusive: false,
                              autoDelete: false, 
                              arguments: null);

        var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

        _channel.BasicPublish(exchange: "", 
                              routingKey: queueName, 
                              basicProperties: null, 
                              body: body);

        Console.WriteLine($" [x] Sent message to queue {queueName}: {message}");
    }

    // Método para consumir mensajes de una cola
    public void ConsumeMessage(string queueName)
    {
        // Declarar la cola en caso de que no exista
        _channel.QueueDeclare(queue: queueName, 
                              durable: true, 
                              exclusive: false, 
                              autoDelete: false, 
                              arguments: null);

        // Crear un consumidor para leer los mensajes
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Aquí puedes procesar el mensaje recibido
            Console.WriteLine($" [x] Received: {message}");

            // Lógica para procesar el mensaje (por ejemplo, actualización de datos, etc.)
            ProcessMessage(message);
        };

        // Escuchar la cola de mensajes
        _channel.BasicConsume(queue: queueName,
                              autoAck: true,  // Si quieres confirmar automáticamente los mensajes
                              consumer: consumer);

        Console.WriteLine($" [*] Waiting for messages in {queueName}. To exit press CTRL+C");
    }

    // Método para procesar el mensaje
    private void ProcessMessage(string message)
    {
        // Implementa la lógica que se debe ejecutar cuando se recibe el mensaje
        // Ejemplo: Deserializar y actualizar algún dato en la base de datos
        Console.WriteLine($"Processing message: {message}");
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}
