using Amazon.DynamoDBv2.DataModel;

namespace webapi.Domain
{
    public class EventTrack
    {
        [DynamoDBProperty("Status")]
        public string Status { get; set; }

        [DynamoDBProperty("EventDate")]
        public string EventDate { get; set; }
    }
}