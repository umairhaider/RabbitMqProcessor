using System.Text;
using RabbitMQ.Client;

namespace HashApiApp.RabbitMq
{
    public class RabbitMqProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqProducer(string connectionString)
        {
            _factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "hashes",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendHash(string hash)
        {
            var body = Encoding.UTF8.GetBytes(hash);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "hashes",
                                  basicProperties: null,
                                  body: body);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
