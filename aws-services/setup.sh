#!/bin/sh
cd /aws-services

# Wait just in case localstack delays the start
sleep 7s

echo "Creating dynamoDB table..."
# Create table in dynamoDB using json(reference https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_CreateTable.html)
aws dynamodb create-table --endpoint-url http://localstack:4566 --cli-input-json file://create-table.json

echo "Creating SQS Queues..."
# Create queues in SQS
aws sqs create-queue --queue-name eventqueueA --endpoint-url http://localstack:4566
aws sqs create-queue --queue-name eventqueueB --endpoint-url http://localstack:4566

echo "Creating SNS Topic..."
# Create a topic in SNS
aws sns create-topic --name eventtopic --endpoint-url http://localstack:4566

echo "Subscribe queues to SNS Topic..."
# Subscribe queues to SNS Topic
aws sns subscribe --topic-arn arn:aws:sns:us-east-1:000000000000:eventtopic --protocol sqs --notification-endpoint http://localstack:4566/000000000000/eventqueueA --endpoint-url http://localstack:4566
aws sns subscribe --topic-arn arn:aws:sns:us-east-1:000000000000:eventtopic --protocol sqs --notification-endpoint http://localstack:4566/000000000000/eventqueueB --endpoint-url http://localstack:4566

echo "Finish localstack setup"