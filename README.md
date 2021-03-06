# dynamodb-event-tracking

## Localstack 

https://hub.docker.com/r/localstack/localstack

### Steps

##### run all containers with docker compose
- run docker-compose -f docker-compose.yml up -d --build

##### check if dynamodb table was created
- run aws dynamodb list-tables --endpoint-url http://localhost:4566

##### check if sqs queues was created
- run aws sqs list-queues --endpoint-url http://localhost:4566

##### check if sns topic was created
- run aws sqs list-subscriptions --endpoint-url http://localhost:4566

##### check message in sqs queues
- run aws sqs receive-message --queue-url http://localhost:4566/000000000000/eventqueueA --attribute-names All --message-attribute-names All --max-number-of-messages 10 --endpoint-url http://localhost:4566 
- run aws sqs receive-message --queue-url http://localhost:4566/000000000000/eventqueueB --attribute-names All --message-attribute-names All --max-number-of-messages 10 --endpoint-url http://localhost:4566