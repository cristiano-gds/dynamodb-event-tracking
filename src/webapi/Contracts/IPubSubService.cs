using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;
using webapi.Model;

namespace webapi.Contracts
{
    public interface IPubSubService
    {
         Task Publish(MessageEventModel message);
    }
}