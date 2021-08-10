using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using webapi.Contracts;
using webapi.Model;
using webapi.Domain;

namespace webapi.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly Table _eventTable;
        private const string TABLE_NAME = "EventTracking";

        public EventRepository(IDynamoDBContext context)
        {
            _context = context;
        }
        
        public async Task<string> Add(EventInput entity)
        {
            var id = Guid.NewGuid().ToString();
            Event model = new Event()
            {
                Id = id,
                ExternalCode = entity.ExternalCode,
                Message = entity.Message,
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Tracking = new List<EventTrack>()
            };

            model.Tracking.Add(new EventTrack(){
                Status = "EVENT_CREATED",
                EventDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            });

            await _context.SaveAsync<Event>(model);

            return id;
        }

        public async Task<Event> Single(string id)
        {
            return await _context.LoadAsync<Event>(id);
        }

        public async Task Update(Event entity)
        {            
            await _context.SaveAsync<Event>(entity);
        }

        public async Task<List<Event>> FindByExternalCode(string externalCode)
        {
            var queryFilter = new QueryFilter("ExternalCode", QueryOperator.Equal, externalCode);
            var search = _context.FromQueryAsync<Event>(
                    new QueryOperationConfig { 
                        IndexName = "ExternalCodeIndex", 
                        Filter = queryFilter});

            List<Event> events = new List<Event>();
            do
            {
                List<Event> nextSet =  await search.GetNextSetAsync();
                events.AddRange(nextSet);
            }
            while (!search.IsDone);

            return events;
        }
    }
}