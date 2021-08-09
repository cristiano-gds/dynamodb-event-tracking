# dynamodb-event-tracking

## Localstack 

https://hub.docker.com/r/localstack/localstack

### Steps

##### execute all docker compose
- run docker-compose -f docker-compose.yml up -d --build

##### check if dynamodb table was created
- run aws dynamodb list-tables --endpoint-url http://localhost:4566

