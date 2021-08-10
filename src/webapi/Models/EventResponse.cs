using System.Collections.Generic;

namespace webapi.Models
{
    public class EventResponse
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public string ExternalCode { get; set; }

        public string Message { get; set; }

        public List<EventTracking> Tracking  { get; set; }
    }
}