using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using webapi.Contracts;
using webapi.Model;

namespace webapi.Infrastructure
{
    public class PubSubSNSService : IPubSubService
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;
        private string topicArn = "arn:aws:sns:us-east-1:000000000000:eventtopic";

        public PubSubSNSService(AmazonSimpleNotificationServiceClient snsClient)
        {
            _snsClient = snsClient;
        }

        public Task Publish(MessageEventModel message)
        {
            return _snsClient.PublishAsync(topicArn, JsonConvert.SerializeObject(message));
        }
    }
}