using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Model;
using webapi.Domain;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Runtime.CompilerServices;
using Amazon.Runtime;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.Model;
using webapi.Contracts;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly AmazonDynamoDBClient _client;
        private readonly IEventRepository _repository;

        public EventController(ILogger<EventController> logger, IEventRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<string> CreateAsync([FromBody]EventInput model)
        {
            //Save event in DynamoDB
            return await _repository.Add(model);

            //publish message in SNS


        }

        [HttpPut]
        [Route("ChangeTracking/{id}")]
        public async void ChangeTrackingAsync(string id, [FromBody]string status)
        {
            Event entity = await _repository.Single(id);

            entity.Tracking.Add(new EventTrack()
            {
                Status = status,
                EventDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            });

            await _repository.Update(entity);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Event> GetAsync(string id)
        {
            return await _repository.Single(id);
        }

        [HttpGet]
        [Route("GetByExternalCode/{code}")]
        public async Task<List<Event>> GetByExternalCodeAsync(string code)
        {
            return await _repository.FindByExternalCode(code);
        }
    }
}
