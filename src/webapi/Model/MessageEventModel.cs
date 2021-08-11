namespace webapi.Model
{
    public class MessageEventModel : EventModel
    {
        public MessageEventModel(string idEventTracking, EventModel model)
        {
            Message = model.Message;
            ExternalCode = model.ExternalCode;
            IdEventTracking = idEventTracking;
        }
        public string IdEventTracking { get; set; }
    }
}