using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace workerB
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AmazonSQSClient _sqsClient;
        private const string QUEUE_URL = "http://localhost:4566/000000000000/eventqueueB";
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            var sqsConfig = new AmazonSQSConfig {ServiceURL = "http://localstack:4566"};
            _sqsClient = new AmazonSQSClient(sqsConfig);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Run workerB...");
                ProcessMessage();

                await Task.Delay(RandomNumber(), stoppingToken);
            }
        }

        private async void ProcessMessage()
        {
            var response = await _sqsClient.ReceiveMessageAsync(QUEUE_URL);
            foreach (var message in response.Messages)
                {
                    UpdateTracking(message);
                    await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest(QUEUE_URL, message.ReceiptHandle));
                }
        }

        private void UpdateTracking(Message message)
        {
            var body = JToken.Parse(message.Body);
            MessageModel messageModel = JsonConvert.DeserializeObject<MessageModel>(body["Message"].ToString());

            Console.WriteLine($"Message received in workerB. ExternalCode: {messageModel.ExternalCode}");

            UpdateInDynamoDB(messageModel);
        }

        private async void UpdateInDynamoDB(MessageModel message)
        {
            AmazonDynamoDBConfig dbConfig = new AmazonDynamoDBConfig();
            dbConfig.ServiceURL = "http://localstack:4566";
            var client = new AmazonDynamoDBClient(dbConfig);
            
            Table table = Table.LoadTable(client, "EventTracking");
            Primitive hash = new Primitive(message.IdEventTracking);
            Document document = await table.GetItemAsync(hash);

            Document updateTrack = new Document();
            updateTrack["Status"] = "EVENT_UPDATED_B";
            updateTrack["EventDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            ((DynamoDBList)document["Tracking"]).Add(updateTrack);

            await table.UpdateItemAsync(document);
        }

        private int RandomNumber()
        {
            Random r = new Random();
            return r.Next(5000, 10000);
        }
    }
}
