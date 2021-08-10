using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace webapi.Domain
{
    [DynamoDBTable("EventTracking")]
    public class Event
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty("Date")]
        public string Date { get; set; }
        
        [DynamoDBProperty("ExternalCode")]
        public string ExternalCode { get; set; }

        [DynamoDBProperty("Message")]
        public string Message { get; set; }

        [DynamoDBProperty("Tracking")]
        public List<EventTrack> Tracking  { get; set; }
    }
}