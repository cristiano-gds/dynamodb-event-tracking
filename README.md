# dynamodb-event-tracking

### execute all docker compose
- run docker-compose -f docker-compose-localstack.yml up -d

### check if dynamodb table was created
- run aws dynamodb list-tables --endpoint-url http://localhost:4566