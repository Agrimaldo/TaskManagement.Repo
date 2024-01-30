
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using TaskManagement.Domain.Util.Enumerators;

namespace TaskManagement.Domain.Interfaces
{
    public interface IMessageBus
    {
        void Publish<T>(MessageType messageType, T content);
        T Consume<T>(MessageType messageType, EventHandler<BasicDeliverEventArgs> receiverMethod);
    }
}
