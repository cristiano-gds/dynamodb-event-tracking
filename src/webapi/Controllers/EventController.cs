using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Runtime.CompilerServices;
using Amazon.Runtime;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.Model;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly AmazonDynamoDBClient _client;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
            AmazonDynamoDBConfig dbConfig = new AmazonDynamoDBConfig();
            dbConfig.ServiceURL = "http://localstack:4566";
            _client = new AmazonDynamoDBClient(dbConfig);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<string> CreateAsync([FromBody]EventRequest eventRequest)
        {
            //Save event in DynamoDB
            
            Table table = Table.LoadTable(_client, "EventTracking");

            var id = Guid.NewGuid().ToString();
            Document document = new Document();
            document["Id"] = id;
            document["Externalcode"] = eventRequest.ExternalCode;
            document["Message"] = eventRequest.Message;
            document["Date"] = DateTime.Now.ToString("yyyy-MM-dd");
            
            var tracking = new DynamoDBList();
        
            Document track = new Document();
            track["Status"] = "EVENT_CREATED";
            track["EventDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            tracking.Add(track);

            document.Add("Tracking", tracking);

            await table.PutItemAsync(document);

            return id;
        }

        [HttpPut]
        [Route("ChangeTracking/{id}")]
        public async void ChangeTrackingAsync(string id, [FromBody]string status)
        {
            Table table = Table.LoadTable(_client, "EventTracking");
            Primitive hash = new Primitive(id);
            Document document = await table.GetItemAsync(hash);
            
            Document updateTrack = new Document();
            updateTrack["Status"] = status;
            updateTrack["EventDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            
            ((DynamoDBList)document["Tracking"]).Add(updateTrack);

            await table.UpdateItemAsync(document);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<EventResponse> GetAsync(string id)
        {
            Table table = Table.LoadTable(_client, "EventTracking");
            Document document = await table.GetItemAsync(id);
            
            return JsonConvert.DeserializeObject<EventResponse>(document.ToJson());
        }

        [HttpGet]
        [Route("GetByExternalCode/{code}")]
        public async Task<List<EventResponse>> GetByExternalCodeAsync(string code)
        {
            List<Document> documents = new List<Document>();

            QueryRequest query = new QueryRequest()
            {
                TableName = "EventTracking",
                IndexName = "ExternalCodeIndex",
                KeyConditionExpression = "ExternalCode = :v_code",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":v_code", new AttributeValue { S = code }}
                },
                ScanIndexForward = true,
                ProjectionExpression = "Id"
            };

            var result = await _client.QueryAsync(query);

            Table table = Table.LoadTable(_client, "EventTracking");
            foreach(var item in result.Items)
            {
                documents.Add(await table.GetItemAsync(item["Id"].S));
            }
            
            return JsonConvert.DeserializeObject<List<EventResponse>>(documents.ToJson());
        }
    }
}
