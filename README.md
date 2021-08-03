# dynamodb-event-tracking

### execute all docker compose
- run docker-compose -f docker-compose-localstack.yml up -d

### configure aws cli 
- run aws configure
- enter access id "test"
- enter access key "test"
- enter region us-east-1

### create dynamodb table
- run aws dynamodb create-table --endpoint-url http://localhost:4566 --cli-input-json file://DB/create-table.json