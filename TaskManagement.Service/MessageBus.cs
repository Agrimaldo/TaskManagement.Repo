using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Util.Enumerators;

namespace TaskManagement.Service
{
    public class MessageBus: IMessageBus, IDisposable
    {
        IConnection connection;
        public IModel channel;

        public MessageBus(ConnectionFactory factory) 
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Publish<T>(MessageType messageType, T content)
        {
            string queueName = $"{messageType.ToString()}.Queue";
            
            channel.QueueDeclare(queue: queueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            string messageContent = JsonSerializer.Serialize(content);
            channel.BasicPublish("", queueName, null, Encoding.UTF8.GetBytes(messageContent));
        }

        public T Consume<T>(MessageType messageType, EventHandler<BasicDeliverEventArgs> receiverMethod)
        {
            T? result = default!;
            string queueName = $"{messageType.ToString()}.Queue";
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel)!;
            
            channel.QueueDeclare(queue: queueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            consumer.Received += receiverMethod += (sender, e) =>
            {
                channel.BasicNack(e.DeliveryTag, false, false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return result;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    channel.Dispose();
                    connection.Dispose();
                }
            }

            _disposed = true;
        }
        #endregion
    }
}